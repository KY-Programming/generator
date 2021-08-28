using KY.Generator.Transfer;

namespace KY.Generator.Templates
{
    public class LinkedUsingTemplate : UsingTemplate
    {
        private readonly TypeTransferObject type;
        private readonly string path;

        public override string Namespace => this.type.Namespace;
        public override string Type => this.type.Name;
        public override string Path => $"{this.path}/{this.type.FileName}";

        public LinkedUsingTemplate(TypeTransferObject type, string path)
        {
            this.type = type;
            this.path = path;
        }
    }
}
