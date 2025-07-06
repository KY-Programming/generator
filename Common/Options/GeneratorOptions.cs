using System.Diagnostics;
using KY.Generator.Languages;
using KY.Generator.Models;
using KY.Generator.Transfer;

namespace KY.Generator;

[DebuggerDisplay("GeneratorOptions for {Target}")]
public class GeneratorOptions(GeneratorOptions? parent, GeneratorOptions? global, object? target = null)
    : OptionsBase<GeneratorOptions>(parent, global, target)
{
    private bool? propertiesToFields;
    private bool? fieldsToProperties;
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
    private bool? skipNamespace;
    private bool? onlySubTypes;
    private string? rename;
    private TypeTransferObject? returnType;
    private string? formatter;
    private bool? forceOverwrite;
    private List<Import>? imports;
    private bool? noOptional;
    private bool? nullable;
    private string? modelOutput;

    public bool PropertiesToFields
    {
        get => this.GetValue(x => x.propertiesToFields);
        set => this.propertiesToFields = value;
    }

    public bool FieldsToProperties
    {
        get => this.GetValue(x => x.fieldsToProperties);
        set => this.fieldsToProperties = value;
    }

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

    public IReadOnlyDictionary<string, string> ReplaceName => this.GetDictionary(x => x.replaceName);

    public FormattingOptions? Formatting
    {
        // TODO: Include all parents formatting options
        get => new(() => this.Language?.Formatting, () => this.formatting, () => this.Global?.formatting, () => this.Parents.FirstOrDefault()?.Formatting);
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

    public void AddToImports(params IEnumerable<Import> values)
    {
        this.imports ??= [];
        this.imports.AddRange(values);
    }
}
