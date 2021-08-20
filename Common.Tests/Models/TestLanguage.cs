using System.Collections.Generic;
using KY.Generator.Languages;
using KY.Generator.Templates;

namespace KY.Generator.Common.Tests.Models
{
    public class TestLanguage : BaseLanguage
    {
        public override string Name => "Test";
        public override bool ImportFromSystem => true;
        public override bool IsGenericTypeWithSameNameAllowed { get; }

        protected override IEnumerable<UsingTemplate> GetUsings(FileTemplate fileTemplate)
        {
            return fileTemplate.GetUsingsByNamespace();
        }
    }
}
