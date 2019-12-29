namespace KY.Generator.Transfer
{
    public class HttpServiceActionParameterTransferObject
    {
        public string Name { get; set; }
        public bool FromBody { get; set; }
        public bool AppendName { get; set; } = true;
        public bool Inline { get; set; }
        public int InlineIndex { get; set; }
        public TypeTransferObject Type { get; set; }
        public bool IsOptional { get; set; }
    }
}