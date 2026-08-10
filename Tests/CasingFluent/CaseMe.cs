namespace CasingFluent;

/// <summary>
/// Left on the default, so every member name is reformatted to the casing of the target language.
/// The six spellings are the ones that behave differently: already lower, all upper, pascal, camel,
/// snake and upper snake. S1 is the digit suffix, which must not be split off the letter.
/// </summary>
public class CaseMe
{
    public string alllower { get; set; } = "";
    public string ALLUPPER { get; set; } = "";
    public string PascalCase { get; set; } = "";
    public string camelCase { get; set; } = "";
    public string snake_case { get; set; } = "";
    public string UPPER_SNAKE_CASE { get; set; } = "";
    public string S1 { get; set; } = "";
}
