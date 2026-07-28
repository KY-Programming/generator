using System;
using System.Linq;
using KY.Core.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KY.Generator.Tests
{
    [TestClass]
    public class VisualStudioParserTests
    {
        private const string SolutionText = @"
Microsoft Visual Studio Solution File, Format Version 12.00
Project(""{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""Alpha"", ""Alpha\Alpha.csproj"", ""{11111111-1111-1111-1111-111111111111}""
EndProject
Global
EndGlobal
";

        // Shape as written by "dotnet sln migrate": projects nested in a folder, forward slashes and no ids at all.
        // The <BuildType> child carries a "Project" attribute and must not be mistaken for a project itself.
        private const string SolutionXml = @"<Solution>
  <Folder Name=""/Tests/"">
    <Project Path=""Alpha/Alpha.csproj"">
      <BuildType Project=""Custom"" />
    </Project>
  </Folder>
  <Project Path=""Beta\Beta.csproj"" Id=""{22222222-2222-2222-2222-222222222222}"" />
</Solution>";

        private VisualStudioParser parser;
        private string directory;

        [TestInitialize]
        public void Initialize()
        {
            this.parser = new VisualStudioParser();
            this.directory = FileSystem.Combine(Environment.CurrentDirectory, "VisualStudioParserTests");
            FileSystem.CreateDirectory(this.directory);
        }

        [TestMethod]
        public void IsSolutionDetectsBothFormats()
        {
            Assert.IsTrue(VisualStudioParser.IsSolution("Some.sln"));
            Assert.IsTrue(VisualStudioParser.IsSolution("Some.slnx"));
            Assert.IsFalse(VisualStudioParser.IsSolution("Some.csproj"));
            Assert.IsTrue(VisualStudioParser.IsXmlSolution("Some.slnx"));
            Assert.IsFalse(VisualStudioParser.IsXmlSolution("Some.sln"), ".sln must not be detected as xml solution");
        }

        [TestMethod]
        public void ParseTextSolutionReadsProjectAndId()
        {
            string path = this.Write("text.sln", SolutionText);

            VisualStudioSolution solution = this.parser.ParseSolution(path);

            Assert.IsNotNull(solution);
            Assert.AreEqual(1, solution.Projects.Count);
            Assert.AreEqual("Alpha", solution.Projects[0].Name);
            Assert.AreEqual(new Guid("11111111-1111-1111-1111-111111111111"), solution.Projects[0].Id);
        }

        [TestMethod]
        public void ParseXmlSolutionReadsNestedAndRootProjects()
        {
            string path = this.Write("xml.slnx", SolutionXml);

            VisualStudioSolution solution = this.parser.ParseSolution(path);

            Assert.IsNotNull(solution);
            Assert.AreEqual(2, solution.Projects.Count, "projects inside a <Folder> have to be found as well");
            Assert.IsTrue(solution.Projects.Any(x => x.Path.EndsWith("Alpha.csproj")));
            Assert.IsTrue(solution.Projects.Any(x => x.Path.EndsWith("Beta.csproj")));
        }

        [TestMethod]
        public void ParseXmlSolutionUsesFileNameAsProjectName()
        {
            string path = this.Write("name.slnx", SolutionXml);

            VisualStudioSolution solution = this.parser.ParseSolution(path);

            Assert.AreEqual("Alpha", solution.Projects.First(x => x.Path.EndsWith("Alpha.csproj")).Name);
            Assert.AreEqual("Beta", solution.Projects.First(x => x.Path.EndsWith("Beta.csproj")).Name);
        }

        [TestMethod]
        public void ParseXmlSolutionReadsOptionalId()
        {
            string path = this.Write("id.slnx", SolutionXml);

            VisualStudioSolution solution = this.parser.ParseSolution(path);

            Assert.AreEqual(new Guid("22222222-2222-2222-2222-222222222222"), solution.Projects.First(x => x.Path.EndsWith("Beta.csproj")).Id,
                            "the optional Id attribute has to be used if present");
            Assert.AreEqual(Guid.Empty, solution.Projects.First(x => x.Path.EndsWith("Alpha.csproj")).Id,
                            "without an Id attribute the id stays empty so the caller falls back to the ProjectGuid");
        }

        [TestMethod]
        public void ParseSolutionReturnsNullForMissingFile()
        {
            Assert.IsNull(this.parser.ParseSolution(FileSystem.Combine(this.directory, "does-not-exist.slnx")));
            Assert.IsNull(this.parser.ParseSolution(FileSystem.Combine(this.directory, "does-not-exist.sln")));
        }

        private string Write(string fileName, string content)
        {
            string path = FileSystem.Combine(this.directory, fileName);
            FileSystem.WriteAllText(path, content);
            return path;
        }
    }
}
