namespace KY.Generator.Meta.Extensions
{
    public static class MetaBlockExtension
    {
        public static MetaBlock SkipStartAndEnd(this MetaBlock block)
        {
            block.Skip = true;
            return block;
        }
    }
}