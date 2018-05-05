using System.Collections.Generic;

namespace KY.Generator.Templates
{
    public class MultilineCodeFragment : CodeFragment
    {
        public List<CodeFragment> Fragments { get; }

        public MultilineCodeFragment()
        {
            this.Fragments = new List<CodeFragment>();
        }

        public MultilineCodeFragment AddLine(CodeFragment code)
        {
            this.Fragments.Add(code);
            return this;
        }

        public MultilineCodeFragment AddBlankLine()
        {
            this.Fragments.Add(Code.BlankLine());
            return this;
        }
    }
}