namespace KY.Generator.Meta
{
    public class MetaBlock : MetaElement
    {
        public IMetaElementList Header { get; }
        public IMetaElementList Elements { get; }
        public bool Skip { get; set; }

        public MetaBlock()
        {
            this.Header = new MetaElementList(this);
            this.Elements = new MetaElementList(this);
        }
    }
}