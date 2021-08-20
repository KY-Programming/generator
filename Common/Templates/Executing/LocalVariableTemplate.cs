using KY.Core;

namespace KY.Generator.Templates
{
    public class LocalVariableTemplate : ChainedCodeFragment
    {
        public string Name { get; }

        public LocalVariableTemplate(string name)
        {
            this.Name = name.AssertIsNotNullOrEmpty(nameof(name));
        }
    }
}