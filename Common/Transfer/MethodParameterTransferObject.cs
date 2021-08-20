namespace KY.Generator.Transfer
{
    public class MethodParameterTransferObject
    {
        public string Name { get; set; }
        public string Comment { get; set; }
        public TypeTransferObject Type { get; set; }
        public bool IsOptional { get; set; }
    }
}