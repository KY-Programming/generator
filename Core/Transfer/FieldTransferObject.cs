namespace KY.Generator.Transfer
{
    public class FieldTransferObject
    {
        public string Name { get; set; }
        public TypeTransferObject Type { get; set; }
        public object Default { get; set; }
        public string Comment { get; set; }

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
