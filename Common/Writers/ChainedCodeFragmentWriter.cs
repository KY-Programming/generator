using System.Collections.Generic;
using System.Linq;
using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class ChainedCodeFragmentWriter : ITemplateWriter
    {
        private readonly IOptions options;
        private readonly List<ChainedCodeFragment> progressedChainedCodeFragments = new ();

        public ChainedCodeFragmentWriter(IOptions options)
        {
            this.options = options;
        }

        public void Write(ICodeFragment fragment, IOutputCache output)
        {
            ChainedCodeFragment chainedCodeFragment = (ChainedCodeFragment)fragment;
            this.progressedChainedCodeFragments.Add(chainedCodeFragment.First());
            bool isFirst = true;
            foreach (ChainedCodeFragment codeFragment in chainedCodeFragment.First().Yield().Cast<ChainedCodeFragment>())
            {
                if (!isFirst)
                {
                    output.Add(codeFragment.Separator);
                }
                isFirst = false;
                output.Add(codeFragment);
                if (codeFragment.NewLineAfter)
                {
                    output.BreakLine();
                }
                if (codeFragment.CloseAfter)
                {
                    output.CloseLine();
                }
                if (codeFragment.BreakAfter)
                {
                    output.BreakLine().ExtraIndent();
                }
            }
        }

        public bool IsProcessed(ChainedCodeFragment fragment)
        {
            return this.progressedChainedCodeFragments.Contains(fragment.First());
        }
    }
}
