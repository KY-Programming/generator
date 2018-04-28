namespace KY.Generator.Meta
{
    public class MetaStatement : MetaElement
    {
        public IMetaFragmentList Code { get; }
        public bool IsClosed { get; set; }

        public MetaStatement(bool isClosed = true)
        {
            this.Code = new MetaFragmentList(this);
            this.IsClosed = isClosed;
        }
    }
}