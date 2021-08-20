using System;
using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Languages
{
    internal class ForwardFileLanguage : EmptyLanguage
    {
        public override string Name => nameof(ForwardFileLanguage);

        public override void Write(FileTemplate file, IOutput output)
        {
            if (file is ForwardFileTemplate forwardFile)
            {
                forwardFile.Language.Write(forwardFile.File, output);
            }
            else
            {
                throw new NotImplementedException($"The method {nameof(Write)} for type {file.GetType().Name} is not implemented in {this.Name}.");
            }
        }
    }
}