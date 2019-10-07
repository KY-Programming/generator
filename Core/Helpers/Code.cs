using KY.Generator.Templates;

namespace KY.Generator
{
    public struct Code
    {
        public static Code Instance { get; } = default;

        public MultilineCodeFragment Multiline()
        {
            return new MultilineCodeFragment();
        }
    }
}