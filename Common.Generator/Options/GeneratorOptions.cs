using System.Diagnostics;
using KY.Generator.Languages;
using KY.Generator.Models;
using KY.Generator.Transfer;

namespace KY.Generator;

[DebuggerDisplay("GeneratorOptions for {Target}")]
public class GeneratorOptions(GeneratorOptions? parent, GeneratorOptions? global, object? target = null)
    : OptionsBase<GeneratorOptions>(parent, global, target)
{
    /// <summary>
    /// The key <see cref="LintSuppression"/> holds the comment under that is used for every language without an own
    /// entry. No language can be named like it, so it can not collide with one
    /// </summary>
    public const string AnyLanguage = "";

    private bool? preferInterfaces;
    private bool? optionalFields;
    private bool? optionalProperties;
    private bool? ignore;
    private bool? formatNames;
    private bool? withOptionalProperties;
    private Dictionary<string, string>? replaceName;
    private FormattingOptions? formatting;
    private ILanguage? language;
    private bool? addHeader;
    private bool? addHeaderVersion;
    private Dictionary<string, string>? lintSuppression;
    private bool? skipNamespace;
    private bool? onlySubTypes;
    private bool? never;
    private string? rename;
    private TypeTransferObject? returnType;
    private string? formatter;
    private bool? forceOverwrite;
    private List<Import>? imports;
    private bool? noOptional;
    private bool? nullable;
    private string? modelOutput;

    public bool PreferInterfaces
    {
        get => this.GetValue(x => x.preferInterfaces);
        set => this.preferInterfaces = value;
    }

    public bool OptionalFields
    {
        get => this.GetValue(x => x.optionalFields);
        set => this.optionalFields = value;
    }

    public bool OptionalProperties
    {
        get => this.GetValue(x => x.optionalProperties);
        set => this.optionalProperties = value;
    }

    public bool Ignore
    {
        get => this.GetOwnValue(x => x.ignore);
        set => this.ignore = value;
    }

    public bool FormatNames
    {
        get => this.GetValue(x => x.formatNames, true);
        set => this.formatNames = value;
    }

    public bool WithOptionalProperties
    {
        get => this.GetValue(x => x.withOptionalProperties);
        set => this.withOptionalProperties = value;
    }

    /// <summary>
    /// The replacements configured on this element itself. The replacements of the surrounding scopes are not
    /// inherited, e.g. a replacement configured on a class does not rename the members of that class
    /// </summary>
    public IReadOnlyDictionary<string, string> ReplaceName => this.GetOwnDictionary(x => x.replaceName);

    public FormattingOptions Formatting
    {
        // TODO: Include all parents formatting options
        get => this.formatting ??= new(() => this.Language?.Formatting, () => this.Global?.Formatting, () => this.Parents.FirstOrDefault()?.Formatting);
        set => this.formatting = value;
    }

    public ILanguage? Language
    {
        get => this.GetValue(x => x.language);
        set => this.language = value;
    }

    public bool AddHeader
    {
        get => this.GetValue(x => x.addHeader, true);
        set => this.addHeader = value;
    }

    /// <summary>
    /// Writes the version of the generator into the <c>&lt;auto-generated&gt;</c> header
    /// </summary>
    public bool AddHeaderVersion
    {
        get => this.GetValue(x => x.addHeaderVersion, true);
        set => this.addHeaderVersion = value;
    }

    /// <summary>
    /// The comments that switch the linter off for a generated file, by the name of the language they are written
    /// for. <see cref="AnyLanguage"/> holds the comment for a language without an own entry. A language found in
    /// neither falls back to the comment of its writer, an empty comment writes none at all.
    /// Use <see cref="AddToLintSuppression"/> to fill and <see cref="GetLintSuppression"/> to read it
    /// </summary>
    public IReadOnlyDictionary<string, string> LintSuppression => this.GetDictionary(x => x.lintSuppression);

    public bool SkipNamespace
    {
        get => this.GetValue(x => x.skipNamespace);
        set => this.skipNamespace = value;
    }

    public bool OnlySubTypes
    {
        get => this.GetOwnValue(x => x.onlySubTypes);
        set => this.onlySubTypes = value;
    }

    /// <summary>
    /// The type must never be generated. Writing it aborts the generation with an error
    /// </summary>
    public bool Never
    {
        get => this.GetOwnValue(x => x.never);
        set => this.never = value;
    }

    public string? Rename
    {
        get => this.GetOwnValue(x => x.rename);
        set => this.rename = value;
    }

    public TypeTransferObject? ReturnType
    {
        get => this.GetOwnValue(x => x.returnType);
        set => this.returnType = value;
    }

    public string? Formatter
    {
        get => this.GetValue(x => x.formatter);
        set => this.formatter = value;
    }

    public bool ForceOverwrite
    {
        get => this.GetValue(x => x.forceOverwrite);
        set => this.forceOverwrite = value;
    }

    public IReadOnlyList<Import> Imports => this.GetList(x => x.imports);

    public bool NoOptional
    {
        get => this.GetValue(x => x.noOptional);
        set => this.noOptional = value;
    }

    public bool Nullable
    {
        get => this.GetValue(x => x.nullable);
        set => this.nullable = value;
    }

    public string? ModelOutput
    {
        get => this.GetValue(x => x.modelOutput);
        set => this.modelOutput = value;
    }

    public void AddToReplaceName(string replace, string with)
    {
        this.replaceName ??= new Dictionary<string, string>();
        this.replaceName[replace] = with;
    }

    /// <summary>
    /// Sets the comment that switches the linter off for a generated file. Pass the <see cref="ILanguage.Name"/> of a
    /// language - e.g. <c>nameof(OutputLanguage.TypeScript)</c> - to replace the comment of that one language only,
    /// or <c>null</c> for every language. A comment set for the language wins over one set for every language, no
    /// matter which of the two was set in the closer scope
    /// </summary>
    public void AddToLintSuppression(string comment, string? language = null)
    {
        this.lintSuppression ??= new Dictionary<string, string>();
        this.lintSuppression[NormalizeLanguage(language)] = comment;
    }

    /// <summary>
    /// Reads the comment that switches the linter off for a file written in the given language. <c>null</c> if
    /// nothing is set for it and the comment of the writer has to be used
    /// </summary>
    public string? GetLintSuppression(string? language)
    {
        IReadOnlyDictionary<string, string> suppressions = this.LintSuppression;
        return suppressions.TryGetValue(NormalizeLanguage(language), out string? forLanguage) ? forLanguage
            : suppressions.TryGetValue(AnyLanguage, out string? forAll) ? forAll
            : null;
    }

    private static string NormalizeLanguage(string? language)
    {
        return language?.ToLowerInvariant() ?? AnyLanguage;
    }

    public void AddToImports(params IEnumerable<Import> values)
    {
        this.imports ??= [];
        this.imports.AddRange(values);
    }
}
