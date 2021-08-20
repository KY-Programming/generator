namespace KY.Generator.Languages
{
    public interface IMappableLanguage : ILanguage
    {
        object Key { get; }
    }
}