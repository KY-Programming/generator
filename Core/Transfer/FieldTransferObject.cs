namespace KY.Generator.Transfer
{
    public class FieldTransferObject
    {
        public string Name { get; set; }
        public TypeTransferObject Type { get; set; }
        public object Default { get; set; }
        public string Comment { get; set; }
    }
}