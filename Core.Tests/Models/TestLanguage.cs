using System.Collections.Generic;
using KY.Generator.Languages;
using KY.Generator.Templates;

namespace KY.Generator.Core.Tests.Models
{
    public class TestLanguage : BaseLanguage
    {
        public override string Name => "Test";
        public override bool ImportFromSystem => true;

        public override string FormatFileName(string fileName, bool isInterface)
        {
            return fileName;
        }

        public override string FormatClassName(string className)
        {
            return className;
        }

        public override string FormatPropertyName(string propertyName)
        {
            return propertyName;
        }

        public override string FormatFieldName(string fieldName)
        {
            return fieldName;
        }

        public override string FormatMethodName(string methodName)
        {
            return methodName;
        }

        protected override IEnumerable<UsingTemplate> GetUsings(FileTemplate fileTemplate)
        {
            return fileTemplate.GetUsingsByNamespace();
        }
    }
}