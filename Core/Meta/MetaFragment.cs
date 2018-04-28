namespace KY.Generator.Meta
{
    public class MetaFragment
    {
        public string Code { get; set; }
        public bool IgnoreSeparator { get; }
        public MetaElement Parent { get; set; }
        public int Level => this.Parent?.Level ?? 0;
        public bool BreakAfter { get; set; }

        public MetaFragment(string code = null, bool ignoreSeparator = false)
        {
            this.Code = code;
            this.IgnoreSeparator = ignoreSeparator;
        }
    }
}