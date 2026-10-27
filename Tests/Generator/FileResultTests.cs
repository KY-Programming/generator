using KY.Core.Dependency;
using KY.Generator.Angular;
using KY.Generator.Angular.Configurations;
using KY.Generator.Angular.Writers;
using KY.Generator.AspDotNet;
using KY.Generator.AspDotNet.Configurations;
using KY.Generator.AspDotNet.Readers;
using KY.Generator.Mappings;
using KY.Generator.Models;
using KY.Generator.Reflection.Readers;
using KY.Generator.Templates;
using KY.Generator.Transfer;
using KY.Generator.TypeScript;
using KY.Generator.TypeScript.Languages;
using KY.Generator.TypeScript.Writers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KY.Generator.Tests;

/// <summary>
/// An action that answers with a file has no JSON body. Read like any other action the content is parsed as JSON and
/// thrown away, so the client could not download it. The reader marks these actions, the service reads them as blob
/// together with the file name from the Content-Disposition header
/// </summary>
[TestClass]
public class FileResultTests
{
    private DependencyResolver resolver = null!;
    private List<ITransferObject> transferObjects = null!;
    private List<FileTemplate> files = null!;

    [TestInitialize]
    public void Initialize()
    {
        this.transferObjects = [];
        this.files = [];
        GeneratorEnvironment environment = new();
        environment.LoadedAssemblies.Add(typeof(FileResultTests).Assembly);
        this.resolver = new DependencyResolver();
        this.resolver.Bind<Options>().ToSingleton();
        this.resolver.Bind<IEnvironment>().To(environment);
        this.resolver.Bind<ITypeMapping>().ToSingleton<TypeMapping>();
        this.resolver.Bind<List<ITransferObject>>().To(this.transferObjects);
        this.resolver.Bind<List<FileTemplate>>().To(this.files);
        this.resolver.Bind<GeneratorTypeLoader>().ToSingleton();
        this.resolver.Bind<ReflectionModelReader>().ToSingleton();
        this.resolver.Bind<IOptionsFactory>().ToSingleton<GeneratorOptionsFactory>();
        this.resolver.Bind<IOptionsFactory>().ToSingleton<TypeScriptOptionsFactory>();
        this.resolver.Bind<IOptionsFactory>().ToSingleton<AngularOptionsFactory>();
        this.resolver.Bind<IOptionsFactory>().ToSingleton<AspDotNetOptionsFactory>();
        Options.Register(() => this.resolver.Get<List<IOptionsFactory>>());
        Options.ClearGlobal();
        Options.GetGlobal<GeneratorOptions>().Language = new TypeScriptLanguage(this.resolver);
    }

    [TestCleanup]
    public void Cleanup()
    {
        Options.ClearGlobal();
    }

    [TestMethod]
    [DataRow(nameof(FileController.Content))]
    [DataRow(nameof(FileController.Stream))]
    [DataRow(nameof(FileController.Physical))]
    [DataRow(nameof(FileController.Download))]
    [DataRow(nameof(FileController.DownloadGeneric))]
    [DataRow(nameof(FileController.Export))]
    public void FileResultsAreRecognized(string actionName)
    {
        HttpServiceActionTransferObject action = this.Read(actionName);

        Assert.IsTrue(action.ReturnsFile);
        Assert.AreEqual("Void", action.ReturnType.Name, "The result type must not be read as model");
    }

    [TestMethod]
    [DataRow(nameof(FileController.Plain))]
    [DataRow(nameof(FileController.Overridden))]
    public void OtherResultsAreNoFiles(string actionName)
    {
        Assert.IsFalse(this.Read(actionName).ReturnsFile);
    }

    [TestMethod]
    public void FileIsReadAsBlobWithFileName()
    {
        this.Read(nameof(FileController.Download));

        string output = this.Write();

        StringAssert.Contains(output, "httpOptions?: {}): Observable<FileDownload> {");
        StringAssert.Contains(output, "httpOptions = { ...this.httpOptions, ...httpOptions, responseType: 'blob', observe: 'response' };");
        StringAssert.Contains(output, "this.http.get<HttpResponse<Blob>>(url, httpOptions).subscribe(result => {");
        StringAssert.Contains(output, "subject.next({ blob: result.body ?? new Blob(), fileName: this.getFileName(result.headers.get(\"Content-Disposition\")) });");
        StringAssert.Contains(output, "import { HttpResponse } from \"@angular/common/http\";");
        StringAssert.Contains(output, "import { FileDownload } from \"../models/file-download\";");
        StringAssert.Contains(output, "private getFileName(contentDisposition: string | null): string | undefined {");
        // Compiles with noUncheckedIndexedAccess, a plain match[1] would be string | undefined there
        StringAssert.Contains(output, "let encoded = /filename\\*=UTF-8''([^;]+)/i.exec(contentDisposition)?.[1];");
        StringAssert.Contains(output, "return /filename=\"?([^\";]+)\"?/i.exec(contentDisposition)?.[1];");
        Assert.IsFalse(output.Contains("fixUndefined(result"), "A blob has no values to fix");
    }

    [TestMethod]
    public void FileCanBePosted()
    {
        this.Read(nameof(FileController.Export));

        string output = this.Write();

        StringAssert.Contains(output, "this.http.post<HttpResponse<Blob>>(url, ids, httpOptions).subscribe(result => {");
    }

    [TestMethod]
    public void FileDownloadTypeIsWrittenOnce()
    {
        this.Read(nameof(FileController.Download), nameof(FileController.Content));

        this.Write();

        FileTemplate file = this.files.Single(x => x.Name.Contains("file-download"));
        Output.FileWriter cache = new(file.Options);
        new TypeScriptFileWriter().Write(file, cache);
        StringAssert.Contains(cache.ToString(), "export type FileDownload = { blob: Blob, fileName: string | undefined }");
    }

    [TestMethod]
    public void JsonActionsAreUnchanged()
    {
        this.Read(nameof(FileController.Plain));

        string output = this.Write();

        Assert.IsFalse(output.Contains("blob"));
        Assert.IsFalse(output.Contains("getFileName"));
        Assert.IsFalse(this.files.Any(x => x.Name.Contains("file-download")));
    }

    private HttpServiceActionTransferObject Read(params string[] actionNames)
    {
        AspDotNetControllerReader reader = this.resolver.Create<AspDotNetControllerReader>();
        reader.Read(new AspDotNetReadConfiguration { Controller = new AspDotNetReadControllerConfiguration { Namespace = typeof(FileController).Namespace, Name = nameof(FileController) } });
        HttpServiceTransferObject controller = this.transferObjects.OfType<HttpServiceTransferObject>().Single();
        controller.Actions.RemoveAll(action => !actionNames.Contains(action.Name));
        return controller.Actions.Single(action => action.Name == actionNames[0]);
    }

    private string Write()
    {
        AngularServiceWriter writer = this.resolver.Create<AngularServiceWriter>();
        // Configured like the service command does it
        AngularWriteServiceConfiguration service = new();
        service.HttpClient.HasGetOptions = new AngularHttpClientMethodOptions();
        service.HttpClient.HasPostOptions = new AngularHttpClientMethodOptions();
        writer.Write(new AngularWriteConfiguration { Service = service, WriteModels = false });
        FileTemplate file = this.files.Single(x => x.Name.Contains("service"));
        Output.FileWriter cache = new(file.Options);
        new TypeScriptFileWriter().Write(file, cache);
        return cache.ToString();
    }
}

[Route("api/file")]
public class FileController : ControllerBase
{
    [HttpGet("content")]
    public FileContentResult Content() => this.File([], "application/zip", "content.zip");

    [HttpGet("stream")]
    public async Task<FileStreamResult> Stream() => await Task.FromResult(this.File(new MemoryStream(), "application/zip", "stream.zip"));

    [HttpGet("physical")]
    public PhysicalFileResult Physical() => this.PhysicalFile("C:\\file.zip", "application/zip");

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(FileContentResult), 200)]
    public IActionResult Download(Guid id) => this.File([], "application/zip", $"{id}.zip");

    [HttpGet("generic")]
    [ProducesResponseType<FileContentResult>(200)]
    public IActionResult DownloadGeneric() => this.File([], "application/zip", "generic.zip");

    [HttpPost("export")]
    [Produces(typeof(FileResult))]
    public IActionResult Export([FromBody] string[] ids) => this.File([], "application/zip", "export.zip");

    [HttpGet("plain")]
    public IActionResult Plain() => this.Ok();

    [HttpGet("overridden")]
    [GenerateMethod(TypeName = "string")]
    public FileContentResult Overridden() => this.File([], "text/plain");
}
