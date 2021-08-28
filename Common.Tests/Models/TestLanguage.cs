using KY.Core.Dependency;
using KY.Generator.Languages;

namespace KY.Generator.Common.Tests.Models
{
    public class TestLanguage : BaseLanguage
    {
        public override string Name => "Test";
        public override bool ImportFromSystem => true;
        public override bool IsGenericTypeWithSameNameAllowed { get; }

        public TestLanguage(IDependencyResolver resolver)
            : base(resolver)
        { }
    }
}
