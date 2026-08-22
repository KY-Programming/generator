using System.Reflection;
using KY.Core;
using KY.Core.DataAccess;
using KY.Core.Dependency;
using KY.Generator.Command;
using KY.Generator.Models;
using KY.Generator.Transfer;
using KY.Generator.TypeScript;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace KY.Generator.Tests;

/// <summary>
/// The chain of <c>ky-generator.json</c> files that <see cref="SettingsService"/> resolves for a project: which files
/// take part, in which order they win over each other, and what ends up in the global options
/// </summary>
[TestClass]
public class SettingsServiceTests
{
    private string root = null!;
    private string applicationData = null!;
    private string projectFile = null!;
    private DependencyResolver resolver = null!;
    private readonly List<string> warnings = [];

    /// <summary>
    /// <c>root/solution/project/Test.csproj</c>, so there are two directories above the project to put a
    /// configuration in and one next to it that must never be read
    /// </summary>
    [TestInitialize]
    public void Initialize()
    {
        this.root = FileSystem.Combine(Path.GetTempPath(), "KY.Generator.Tests", Guid.NewGuid().ToString("N"));
        this.applicationData = FileSystem.Combine(this.root, "AppData");
        this.projectFile = FileSystem.Combine(this.root, "solution", "project", "Test.csproj");
        FileSystem.CreateDirectory(this.applicationData);
        FileSystem.CreateDirectory(FileSystem.Parent(this.projectFile));
        FileSystem.WriteAllText(this.projectFile, "<Project />");

        this.resolver = new DependencyResolver();
        this.resolver.Bind<IOptionsFactory>().ToSingleton<GeneratorOptionsFactory>();
        this.resolver.Bind<IOptionsFactory>().ToSingleton<TypeScriptOptionsFactory>();
        Options.Register(() => this.resolver.Get<List<IOptionsFactory>>());
        Options.ClearGlobal();
        SettingsService.ClearCache();
        Logger.Added += this.OnLoggerAdded;
    }

    [TestCleanup]
    public void Cleanup()
    {
        Logger.Added -= this.OnLoggerAdded;
        this.warnings.Clear();
        Options.ClearGlobal();
        SettingsService.ClearCache();
        FileSystem.DeleteDirectory(this.root);
    }

    [TestMethod]
    public void ReadsTheConfigurationAboveTheProject()
    {
        this.Write("solution", @"{ ""options"": { ""addHeader"": false } }");

        this.Apply();

        Assert.IsFalse(Options.GetGlobal<GeneratorOptions>().AddHeader);
    }

    [TestMethod]
    public void IgnoresTheConfigurationNextToTheProject()
    {
        // At project level the assembly attributes are the way to configure the generator
        this.Write(FileSystem.Combine("solution", "project"), @"{ ""options"": { ""addHeader"": false } }");

        this.Apply();

        Assert.IsTrue(Options.GetGlobal<GeneratorOptions>().AddHeader);
    }

    [TestMethod]
    public void TheFileCloserToTheProjectWins()
    {
        this.Write(".", @"{ ""options"": { ""modelOutput"": ""FromTheTop"" } }");
        this.Write("solution", @"{ ""options"": { ""modelOutput"": ""FromTheSolution"" } }");

        this.Apply();

        Assert.AreEqual("FromTheSolution", Options.GetGlobal<GeneratorOptions>().ModelOutput);
    }

    [TestMethod]
    public void WhatTheCloserFileDoesNotSetIsInheritedFromAbove()
    {
        this.Write(".", @"{ ""options"": { ""addHeader"": false, ""modelOutput"": ""FromTheTop"" } }");
        this.Write("solution", @"{ ""options"": { ""modelOutput"": ""FromTheSolution"" } }");

        this.Apply();

        Assert.IsFalse(Options.GetGlobal<GeneratorOptions>().AddHeader);
        Assert.AreEqual("FromTheSolution", Options.GetGlobal<GeneratorOptions>().ModelOutput);
    }

    [TestMethod]
    public void RootStopsTheSearch()
    {
        this.Write(".", @"{ ""options"": { ""addHeader"": false } }");
        this.Write("solution", @"{ ""root"": true }");

        this.Apply();

        Assert.IsTrue(Options.GetGlobal<GeneratorOptions>().AddHeader);
    }

    [TestMethod]
    public void RootDoesNotStopTheGlobalConfiguration()
    {
        this.WriteGlobal(@"{ ""options"": { ""addHeader"": false } }");
        this.Write("solution", @"{ ""root"": true }");

        this.Apply();

        Assert.IsFalse(Options.GetGlobal<GeneratorOptions>().AddHeader);
    }

    [TestMethod]
    public void IgnoreGlobalSettingsSkipsTheApplicationData()
    {
        this.WriteGlobal(@"{ ""options"": { ""addHeader"": false } }");
        this.Write("solution", @"{ ""ignoreGlobalSettings"": true }");

        this.Apply();

        Assert.IsTrue(Options.GetGlobal<GeneratorOptions>().AddHeader);
    }

    [TestMethod]
    public void TheGlobalConfigurationIsTheOutermostLayer()
    {
        this.WriteGlobal(@"{ ""options"": { ""modelOutput"": ""FromTheMachine"" } }");
        this.Write("solution", @"{ ""options"": { ""modelOutput"": ""FromTheSolution"" } }");

        this.Apply();

        Assert.AreEqual("FromTheSolution", Options.GetGlobal<GeneratorOptions>().ModelOutput);
    }

    [TestMethod]
    public void ASectionIsWrittenToTheOptionsOfItsOwnModule()
    {
        this.Write("solution", @"{ ""typescript"": { ""noIndex"": true } }");

        this.Apply();

        Assert.IsTrue(Options.GetGlobal<TypeScriptOptions>().NoIndex);
    }

    [TestMethod]
    public void ANestedObjectIsWrittenToTheOptionsItBelongsTo()
    {
        this.Write("solution", @"{ ""options"": { ""formatting"": { ""indentCount"": 2, ""quote"": ""'"" } } }");

        this.Apply();

        Assert.AreEqual(2, Options.GetGlobal<GeneratorOptions>().Formatting.IndentCount);
        Assert.AreEqual("'", Options.GetGlobal<GeneratorOptions>().Formatting.Quote);
    }

    [TestMethod]
    public void DictionariesAreMergedByKey()
    {
        this.Write(".", @"{ ""options"": { ""lintSuppression"": { """": ""// generated"", ""typescript"": ""/* from the top */"" } } }");
        this.Write("solution", @"{ ""options"": { ""lintSuppression"": { ""typescript"": ""/* eslint-disable */"" } } }");

        this.Apply();

        GeneratorOptions options = Options.GetGlobal<GeneratorOptions>();
        Assert.AreEqual("/* eslint-disable */", options.GetLintSuppression("typescript"));
        Assert.AreEqual("// generated", options.GetLintSuppression("csharp"));
    }

    [TestMethod]
    public void TheTopLevelKeysAreRead()
    {
        Guid license = Guid.NewGuid();
        this.Write("solution", $@"{{ ""api"": ""https://localhost:5001/"", ""statistics"": false, ""license"": ""{license}"" }}");

        SettingsService settingsService = this.Create();

        Assert.AreEqual("https://localhost:5001", settingsService.Api);
        Assert.IsFalse(settingsService.Statistics);
        Assert.AreEqual(license, settingsService.License);
    }

    [TestMethod]
    public void WithoutAnyFileEverythingFallsBackToTheDefaults()
    {
        SettingsService settingsService = this.Create();

        Assert.AreEqual(SettingsService.DefaultApi, settingsService.Api);
        Assert.IsTrue(settingsService.Statistics);
        Assert.AreEqual(0, settingsService.Files.Count);
    }

    /// <summary>
    /// Without a license the generator would ask the api for a new id on every single run
    /// </summary>
    [TestMethod]
    public void TheInstallationIdIsCreatedOnceAndKeptInTheApplicationData()
    {
        Guid created = this.Create().License;

        Assert.AreNotEqual(Guid.Empty, created);
        SettingsService.ClearCache();
        Assert.AreEqual(created, this.Create().License, "The id has to be read back, not generated again");
        Assert.IsTrue(FileSystem.FileExists(FileSystem.Combine(this.applicationData, SettingsService.FileName)));
    }

    /// <summary>
    /// A file that cuts the settings of the machine off can not keep the id there either, and it must not be
    /// written into the repository the file belongs to. It goes into a file of its own in the application data,
    /// named after the path of the settings file that opted out
    /// </summary>
    [TestMethod]
    public void TheInstallationIdOfAFileThatIgnoresTheMachineIsKeptNextToIt()
    {
        this.Write("solution", @"{ ""ignoreGlobalSettings"": true }");

        Guid created = this.Create().License;

        Assert.AreNotEqual(Guid.Empty, created);
        Assert.IsFalse(FileSystem.FileExists(FileSystem.Combine(this.applicationData, SettingsService.FileName)),
                       "The settings of the machine are ignored, so nothing may be written to them");
        Assert.AreEqual("{ \"ignoreGlobalSettings\": true }", FileSystem.ReadAllText(FileSystem.Combine(this.root, "solution", SettingsService.FileName)),
                        "The file in the tree is checked in - a run must never write into it");
        SettingsService.ClearCache();
        Assert.AreEqual(created, this.Create().License, "The id has to be read back, not generated again");
    }

    [TestMethod]
    public void ALicenseFromAFileWinsOverTheInstallationId()
    {
        Guid license = Guid.NewGuid();
        this.Write("solution", $@"{{ ""license"": ""{license}"" }}");

        Assert.AreEqual(license, this.Create().License);
        Assert.IsFalse(FileSystem.FileExists(FileSystem.Combine(this.applicationData, SettingsService.FileName)),
                       "Nothing has to be created while a license is named");
    }

    /// <summary>
    /// The versions before the settings files kept both in <c>global.settings.json</c>
    /// </summary>
    [TestMethod]
    public void TheInstallationIdAndTheStatisticsOfAnOlderVersionAreCarriedOver()
    {
        Guid license = Guid.NewGuid();
        FileSystem.WriteAllText(FileSystem.Combine(this.applicationData, "global.settings.json"),
                                $@"{{ ""StatisticsEnabled"": false, ""License"": ""{license}"" }}");

        SettingsService settingsService = this.Create();

        Assert.AreEqual(license, settingsService.License);
        Assert.IsFalse(settingsService.Statistics);
        Assert.IsTrue(FileSystem.FileExists(FileSystem.Combine(this.applicationData, "global.settings.json")),
                      "The old file stays, an older generator on the same machine still reads it");
    }

    [TestMethod]
    public void AnUnknownKeyIsReportedAndTheRestStillApplies()
    {
        this.Write("solution", @"{ ""nonsense"": 1, ""options"": { ""addHeder"": false, ""addHeader"": false } }");

        SettingsService settingsService = this.Apply();
        settingsService.WarnAboutUnknownKeys(true);

        Assert.IsTrue(this.warnings.Any(warning => warning.Contains("'nonsense'")), "The unknown entry was not reported");
        Assert.IsTrue(this.warnings.Any(warning => warning.Contains("'options.addHeder'")), "The misspelled option was not reported");
        Assert.IsFalse(Options.GetGlobal<GeneratorOptions>().AddHeader, "The options next to the misspelled one have to be applied");
    }

    /// <summary>
    /// A run that generates nothing but C# never loads the TypeScript module, and a <c>typescript</c> section in the
    /// file it reads is not a mistake. Only a command that loaded every module may call an unknown section a warning
    /// </summary>
    [TestMethod]
    public void ASectionOfAModuleThatIsNotLoadedIsNotWarnedAbout()
    {
        this.resolver = new DependencyResolver();
        this.resolver.Bind<IOptionsFactory>().ToSingleton<GeneratorOptionsFactory>();
        Options.Register(() => this.resolver.Get<List<IOptionsFactory>>());
        this.Write("solution", @"{ ""typescript"": { ""noIndex"": true } }");

        this.Apply().WarnAboutUnknownKeys();

        Assert.AreEqual(0, this.warnings.Count, "Nothing may be reported: " + string.Join(", ", this.warnings));
    }

    [TestMethod]
    public void ABrokenFileIsReportedAndIgnored()
    {
        this.Write(".", @"{ ""options"": { ""addHeader"": false } }");
        this.Write("solution", "{ this is not json");

        this.Apply();

        Assert.IsTrue(this.warnings.Any(warning => warning.Contains("Could not read")), "The broken file was not reported");
        Assert.IsFalse(Options.GetGlobal<GeneratorOptions>().AddHeader, "The files above the broken one still have to apply");
    }

    /// <summary>
    /// It applies nothing, but settings-validate has to be able to report it instead of staying quiet about a file
    /// that is right there in the tree
    /// </summary>
    [TestMethod]
    public void ABrokenFileStaysInTheChainWithItsError()
    {
        this.Write("solution", "{ this is not json");

        SettingsFile broken = this.Create().Files.Single();

        Assert.IsNotNull(broken.Error);
        Assert.AreEqual(0, broken.Content.Count, "A file that could not be read must not carry any settings");
    }

    /// <summary>
    /// Writing it would replace everything a hand-written file that only has a typo in it still contains, so the
    /// commands that write have to refuse instead
    /// </summary>
    [TestMethod]
    public void ABrokenFileCanNotBeLoadedForWriting()
    {
        this.Write("solution", "{ this is not json");
        string path = this.Create().GetWritablePath(false, null);

        Assert.AreEqual(FileSystem.Combine(this.root, "solution", SettingsService.FileName), path, "The nearest file is still the one to write");
        Assert.IsFalse(SettingsWriter.TryLoad(path, out JObject content, out string? error));
        Assert.IsNotNull(error);
        Assert.AreEqual(0, content.Count);
    }

    [TestMethod]
    public void CommentsAreAllowed()
    {
        this.Write("solution", "{\n  // the header only adds noise to the diff\n  \"options\": { \"addHeader\": false }\n}");

        this.Apply();

        Assert.IsFalse(Options.GetGlobal<GeneratorOptions>().AddHeader);
    }

    private SettingsService Apply()
    {
        SettingsService settingsService = this.Create();
        settingsService.Apply();
        return settingsService;
    }

    private SettingsService Create()
    {
        SettingsService settingsService = new(new TestEnvironment(this.applicationData), this.resolver);
        settingsService.SetProject(this.projectFile);
        settingsService.Enable();
        return settingsService;
    }

    private void Write(string relativeDirectory, string content)
    {
        FileSystem.WriteAllText(FileSystem.Combine(this.root, relativeDirectory, SettingsService.FileName), content);
    }

    private void WriteGlobal(string content)
    {
        FileSystem.WriteAllText(FileSystem.Combine(this.applicationData, SettingsService.FileName), content);
    }

    private void OnLoggerAdded(object sender, EventArgs<LogEntry> args)
    {
        if (args.Value.Type == LogType.Warning)
        {
            this.warnings.Add(args.Value.Message);
        }
    }

    private class TestEnvironment(string applicationData) : IEnvironment
    {
        public Guid OutputId { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<ITransferObject> TransferObjects { get; } = [];
        public string OutputPath { get; set; } = string.Empty;
        public string ApplicationData { get; } = applicationData;
        public string LocalApplicationData => this.ApplicationData;
        public List<CliCommandParameter> Parameters { get; } = [];
        public bool IsBeforeBuild { get; set; }
        public bool IsMsBuild { get; set; }
        public bool Force { get; set; }
        public List<Assembly> LoadedAssemblies { get; } = [];
        public List<string> RunAtSuccess { get; } = [];
        public List<string> RunAtFailure { get; } = [];
    }
}
