using KY.Core;

namespace KY.Generator.Transfer
{
    public class FieldTransferObject : MemberTransferObject
    {
        public object Default { get; set; }

        public FieldTransferObject()
        { }

        public FieldTransferObject(FieldTransferObject field)
        {
            this.Name = field.Name;
            this.Type = field.Type.Clone();
            this.Default = field.Default;
            this.Comment = field.Comment;
        }
    }
}
