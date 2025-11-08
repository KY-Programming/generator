using KY.Generator.Transfer;

namespace KY.Generator.Templates
{
    public class LinkedTypeTemplate : TypeTemplate
    {
        private readonly TypeTransferObject type;

        public override string Name => this.type.Name;
        public override string Namespace => this.type.Namespace;
        public override bool FromSystem => this.type.FromSystem;
        public override bool IsInterface => this.type.IsInterface;
        public override bool IsNullable => this.type.IsNullable;

        public LinkedTypeTemplate(TypeTransferObject type)
        {
            this.type = type;
        }
    }
}