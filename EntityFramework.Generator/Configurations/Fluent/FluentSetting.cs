namespace KY.Generator.EntityFramework.Configurations.Fluent
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