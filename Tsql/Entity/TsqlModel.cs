using System.Collections.Generic;
using KY.Generator.Tsql.Fluent;

namespace KY.Generator.Tsql.Entity
{
    public class TsqlModel
    {
        public TsqlEntity Entity { get; set; }
        public string RelativePath { get; set; }
        public string Namespace { get; set; }
        public string BasedOn { get; set; }
        public List<IFluentLanguageElement> Fluent { get; }
        public List<string> Usings { get; }
        public List<TsqlModelKeyAction> KeyActions { get; }

        public TsqlModel()
        {
            this.Fluent = new List<IFluentLanguageElement>();
            this.Usings = new List<string>();
            this.KeyActions = new List<TsqlModelKeyAction>();
        }
    }
}