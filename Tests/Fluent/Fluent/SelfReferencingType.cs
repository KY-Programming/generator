namespace Types
{
    public class SelfReferencingType
    {
        public string Property { get; set; }
        public SelfReferencingType Self { get; set; }
    }
}
