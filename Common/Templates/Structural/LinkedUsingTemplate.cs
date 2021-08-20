using System;
using KY.Generator.Transfer;

namespace KY.Generator.Templates
{
    public class LinkedUsingTemplate : UsingTemplate
    {
        private readonly TypeTransferObject type;
        private readonly Func<string> fileNameAction;

        public override string Namespace => this.type.Namespace;
        public override string Type => this.type.Name;
        public override string Path => this.fileNameAction();

        public LinkedUsingTemplate(TypeTransferObject type, Func<string> fileNameAction)
        {
            this.type = type;
            this.fileNameAction = fileNameAction;
        }
    }
}
