namespace KY.Generator.Transfer
{
    public class MemberTransferObject
    {
        public string Name { get; set; }
        public TypeTransferObject Type { get; set; }
        public string Comment { get; set; }
        public bool IsAbstract { get; set; }
        public bool IsVirtual { get; set; }
        public bool IsOverwrite { get; set; }
        public object Default { get; set; }
        public bool IsOptional { get; set; } = true;
    }
}
