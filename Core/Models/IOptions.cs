namespace KY.Generator.Models
{
    public interface IOptions
    {
        bool Strict { get; }
        bool PropertiesToFields { get; }
        bool FieldsToProperties { get; }
        bool PreferInterfaces { get; }
        bool OptionalFields { get; }
        bool OptionalProperties { get; }
        bool Ignore { get; }
    }
}
