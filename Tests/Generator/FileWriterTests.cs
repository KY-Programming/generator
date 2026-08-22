using KY.Core.Dependency;
using KY.Generator.Csharp.Languages;
using KY.Generator.Csharp.Writers;
using KY.Generator.Languages;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.TypeScript.Languages;
using KY.Generator.TypeScript.Writers;
using KY.Generator.Writers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KY.Generator.Tests
{
    /// <summary>
    /// The header and the linter comment are the first lines of every generated file, and nothing that
    /// compiles or type-checks the output ever looks at them - see <see cref="FileWriter" />.
    /// </summary>
    [TestClass]
    public class FileWriterTests
    {
        private DependencyResolver resolver = null!;

        [TestInitialize]
        public void Initialize()
        {
            this.resolver = new DependencyResolver();
        }

        [TestMethod]
        public void TypeScriptWritesTheEslintCommentAndNoTslint()
        {
            string output = this.Write(new TypeScriptFileWriter(), new TypeScriptLanguage(this.resolver));

            StringAssert.Contains(output, "/* eslint-disable */");
            Assert.IsFalse(output.Contains("tslint:disable"), "The dead tslint comment must not be written any more");
        }

        [TestMethod]
        public void CsharpWritesTheResharperComment()
        {
            string output = this.Write(new CsharpFileWriter(), new CsharpLanguage(this.resolver));

            StringAssert.Contains(output, "// ReSharper disable All");
        }

        [TestMethod]
        public void LintSuppressionReplacesTheCommentOfTheLanguage()
        {
            string output = this.Write(new TypeScriptFileWriter(), new TypeScriptLanguage(this.resolver), options => options.AddToLintSuppression("// @ts-nocheck"));

            StringAssert.Contains(output, "// @ts-nocheck");
            Assert.IsFalse(output.Contains("eslint-disable"), "The comment of the language is replaced, not extended");
        }

        [TestMethod]
        public void AnEmptyLintSuppressionWritesNoComment()
        {
            string output = this.Write(new TypeScriptFileWriter(), new TypeScriptLanguage(this.resolver), options => options.AddToLintSuppression(string.Empty));

            Assert.IsFalse(output.Contains("eslint-disable"), "An empty comment switches the linter comment off");
        }

        [TestMethod]
        public void LintSuppressionWithoutALanguageReachesEveryLanguage()
        {
            string output = this.Write(new CsharpFileWriter(), new CsharpLanguage(this.resolver), options => options.AddToLintSuppression("// custom"));

            StringAssert.Contains(output, "// custom");
            Assert.IsFalse(output.Contains("ReSharper disable All"), "The comment of the language is replaced, not extended");
        }

        [TestMethod]
        public void LintSuppressionForOneLanguageReplacesTheCommentOfThatLanguage()
        {
            string output = this.Write(new TypeScriptFileWriter(), new TypeScriptLanguage(this.resolver), options => options.AddToLintSuppression("// @ts-nocheck", nameof(OutputLanguage.TypeScript)));

            StringAssert.Contains(output, "// @ts-nocheck");
            Assert.IsFalse(output.Contains("eslint-disable"), "The comment of the language is replaced, not extended");
        }

        [TestMethod]
        public void LintSuppressionForOneLanguageLeavesTheOtherLanguagesAlone()
        {
            string output = this.Write(new CsharpFileWriter(), new CsharpLanguage(this.resolver), options => options.AddToLintSuppression("// @ts-nocheck", nameof(OutputLanguage.TypeScript)));

            StringAssert.Contains(output, "// ReSharper disable All");
            Assert.IsFalse(output.Contains("@ts-nocheck"), "A comment set for TypeScript must not reach a C# file");
        }

        [TestMethod]
        public void LintSuppressionForOneLanguageWinsOverTheOneForEveryLanguage()
        {
            string output = this.Write(new TypeScriptFileWriter(), new TypeScriptLanguage(this.resolver), options =>
            {
                options.AddToLintSuppression("// custom");
                options.AddToLintSuppression("// @ts-nocheck", nameof(OutputLanguage.TypeScript));
            });

            StringAssert.Contains(output, "// @ts-nocheck");
            Assert.IsFalse(output.Contains("// custom"), "The comment of the language is the more specific one");
        }

        [TestMethod]
        public void TheHeaderCarriesTheVersionByDefault()
        {
            string output = this.Write(new CsharpFileWriter(), new CsharpLanguage(this.resolver));

            StringAssert.Matches(output, new System.Text.RegularExpressions.Regex(@"This code was generated with \S+ \d+\.\d+"));
        }

        [TestMethod]
        public void AddHeaderVersionWritesTheGeneratorWithoutItsVersion()
        {
            string output = this.Write(new CsharpFileWriter(), new CsharpLanguage(this.resolver), options => options.AddHeaderVersion = false);

            StringAssert.Contains(output, "This code was generated with");
            StringAssert.DoesNotMatch(output, new System.Text.RegularExpressions.Regex(@"This code was generated with \S+ \d+\.\d+"));
        }

        private string Write(ITemplateWriter writer, ILanguage language, Action<GeneratorOptions>? configure = null)
        {
            GeneratorOptions options = new(null, null);
            options.Language = language;
            configure?.Invoke(options);
            FileTemplate template = new("Output", options) { Name = "example" };
            Output.FileWriter cache = new(options);
            writer.Write(template, cache);
            return cache.ToString();
        }
    }
}
