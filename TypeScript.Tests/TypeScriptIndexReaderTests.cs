using KY.Core;
using KY.Generator.Models;
using KY.Generator.TypeScript.Transfer;
using KY.Generator.TypeScript.Transfer.Readers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KY.Generator.TypeScript.Tests
{
    [TestClass]
    public class TypeScriptIndexReaderTests
    {
        private IEnvironment environment = new Environment();

        [TestMethod]
        public void Test()
        {
            string file = @"
export * from './lib/test/file1';
export * from './lib/test/file-2'
export * from ""./lib/test/file3"";
// Comment here
export { Type } from ""./lib/test/file4"";
export { Type1, Type2 } from ""./lib/test/file5"";
export { 
    Type1, 
    Type2, 
    Type3
} from './lib/test/file6';";
            TypeScriptIndexReader reader = new(this.environment);
            TypeScriptIndexFile indexFile = reader.Parse(file);
            Assert.AreEqual(7, indexFile.Lines.Count, "Lines count does not match");
            Assert.AreEqual("*", indexFile.Lines[0].CastTo<ExportIndexLine>().Types[0]);
            Assert.AreEqual("./lib/test/file1", indexFile.Lines[0].CastTo<ExportIndexLine>().Path);
            Assert.AreEqual("*", indexFile.Lines[1].CastTo<ExportIndexLine>().Types[0]);
            Assert.AreEqual("./lib/test/file-2", indexFile.Lines[1].CastTo<ExportIndexLine>().Path);
            Assert.AreEqual("*", indexFile.Lines[2].CastTo<ExportIndexLine>().Types[0]);
            Assert.AreEqual("./lib/test/file3", indexFile.Lines[2].CastTo<ExportIndexLine>().Path);
            Assert.AreEqual("// Comment here", indexFile.Lines[3].CastTo<UnknownIndexLine>().Content);
            Assert.AreEqual("Type", indexFile.Lines[4].CastTo<ExportIndexLine>().Types[0]);
            Assert.AreEqual("./lib/test/file4", indexFile.Lines[4].CastTo<ExportIndexLine>().Path);
            Assert.AreEqual("Type1", indexFile.Lines[5].CastTo<ExportIndexLine>().Types[0]);
            Assert.AreEqual("Type2", indexFile.Lines[5].CastTo<ExportIndexLine>().Types[1]);
            Assert.AreEqual("./lib/test/file5", indexFile.Lines[5].CastTo<ExportIndexLine>().Path);
            Assert.AreEqual("Type1", indexFile.Lines[6].CastTo<ExportIndexLine>().Types[0]);
            Assert.AreEqual("Type2", indexFile.Lines[6].CastTo<ExportIndexLine>().Types[1]);
            Assert.AreEqual("Type3", indexFile.Lines[6].CastTo<ExportIndexLine>().Types[2]);
            Assert.AreEqual("./lib/test/file6", indexFile.Lines[6].CastTo<ExportIndexLine>().Path);
        }

    }
}
