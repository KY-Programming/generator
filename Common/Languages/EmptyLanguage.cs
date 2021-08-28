using System;
using System.Collections.Generic;
using System.Linq;
using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Languages
{
    public class EmptyLanguage : ILanguage
    {
        public virtual string Name => "Empty";
        public bool ImportFromSystem => true;
        public Dictionary<string, string> ReservedKeywords { get; } = new();
        public bool IsGenericTypeWithSameNameAllowed => true;
        public FormattingOptions Formatting { get; } = new();

        public virtual void Write(ICodeFragment code, IOutputCache output)
        {
            throw new NotImplementedException($"The method {nameof(Write)} for type {code.GetType().Name} is not implemented in {this.Name}.");
        }

        public void Write(IEnumerable<ICodeFragment> code, IOutputCache output)
        {
            throw new NotImplementedException($"The method {nameof(Write)} for type {code.FirstOrDefault()?.GetType().Name} is not implemented in {this.Name}.");
        }

        public string FormatFile(string name, IOptions options, string type = null, bool force = false)
        {
            return Formatter.Format(name, options.Formatting.FileCase, options, force);
        }
    }
}
