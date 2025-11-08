namespace KY.Generator;

public class FluentIgnore : IFluentLanguageElement
{
    public string Property { get; }

    public FluentIgnore(string property)
    {
        this.Property = property;
    }
}
