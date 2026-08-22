using KY.Core.DataAccess;
using KY.Generator.Documentation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace KY.Generator.Tests
{
    /// <summary>
    /// Documentation based options (e.g. "Generator ignore") only work if the xml documentation of an assembly is
    /// found. An assembly from a package reference is resolved out of the build output, where the SDK does not copy
    /// the package's xml file to - see <see cref="DocumentationReader" />.
    /// </summary>
    [TestClass]
    public class DocumentationReaderTests
    {
        [TestMethod]
        public void DocumentationNextToTheAssemblyIsRead()
        {
            Assert.IsTrue(FileSystem.FileExists(typeof(GenerateNeverAttribute).Assembly.Location.Replace(".dll", ".xml")), "Precondition: the documentation is copied next to the assembly");
            StringAssert.StartsWith(DocumentationReader.Get(typeof(GenerateNeverAttribute)), "Marks a type that must never be generated.");
        }

        [TestMethod]
        public void DocumentationOfAPackageAssemblyIsReadFromThePackageCache()
        {
            Assert.IsFalse(FileSystem.FileExists(typeof(JsonConvert).Assembly.Location.Replace(".dll", ".xml")), "Precondition: the documentation of a package is not copied next to the assembly");
            Assert.AreEqual("Provides methods for converting between .NET types and JSON types.", DocumentationReader.Get(typeof(JsonConvert)));
        }
    }
}
