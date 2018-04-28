namespace KY.Generator.Meta
{
    public abstract class MetaElement
    {
        public MetaElement Parent { get; set; }
        public int Level => (this.UseParentLevel ? this.Parent?.Level : this.Parent?.Level + 1) ?? 0;
        public bool UseParentLevel { get; set; }
    }
}