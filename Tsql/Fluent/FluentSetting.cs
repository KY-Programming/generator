namespace KY.Generator.Tsql.Fluent
{
    public class FluentIgnore : IFluentLanguageElement
    {
        public string Property { get; }

        public FluentIgnore(string property)
        {
            this.Property = property;
        }
    }
}