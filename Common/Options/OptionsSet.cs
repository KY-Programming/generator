using System.Collections.Generic;
using KY.Core;
using KY.Generator.Languages;

namespace KY.Generator
{
    public class OptionsSet : OptionsSetBase<OptionsSet, OptionsPart>, IOptions
    {
        public static OptionsSet GlobalInstance { get; } = new(null, null, null, "global");

        bool IOptions.Strict
        {
            get => this.GetPrimitive(x => x?.Strict);
            set => this.Part.Strict = value;
        }

        public bool IsStrictSet => this.GetValue(x => x?.Strict) != null;

        bool IOptions.PropertiesToFields
        {
            get => this.GetPrimitive(x => x?.PropertiesToFields);
            set => this.Part.PropertiesToFields = value;
        }

        bool IOptions.FieldsToProperties
        {
            get => this.GetPrimitive(x => x?.FieldsToProperties);
            set => this.Part.FieldsToProperties = value;
        }

        bool IOptions.PreferInterfaces
        {
            get => this.GetPrimitive(x => x?.PreferInterfaces);
            set => this.Part.PreferInterfaces = value;
        }

        bool IOptions.OptionalFields
        {
            get => this.GetPrimitive(x => x?.OptionalFields);
            set => this.Part.OptionalFields = value;
        }

        bool IOptions.OptionalProperties
        {
            get => this.GetPrimitive(x => x?.OptionalProperties);
            set => this.Part.OptionalProperties = value;
        }

        bool IOptions.Ignore
        {
            get => this.GetPrimitive(x => x?.Ignore);
            set => this.Part.Ignore = value;
        }

        bool IOptions.FormatNames
        {
            get => this.GetPrimitive(x => x?.FormatNames, true);
            set => this.Part.FormatNames = value;
        }

        bool IOptions.WithOptionalProperties
        {
            get => this.GetPrimitive(x => x?.WithOptionalProperties);
            set => this.Part.WithOptionalProperties = value;
        }

        bool IOptions.AddHeader
        {
            get => this.GetPrimitive(x => x?.AddHeader, true);
            set => this.Part.AddHeader = value;
        }

        bool IOptions.SkipNamespace
        {
            get => this.GetPrimitive(x => x?.SkipNamespace);
            set => this.Part.SkipNamespace = value;
        }

        bool IOptions.OnlySubTypes
        {
            get => this.Part.OnlySubTypes ?? this.Global?.Part.OnlySubTypes ?? false;
            set => this.Part.OnlySubTypes = value;
        }

        bool IOptions.NoIndex
        {
            get => this.Part.NoIndex ?? this.Global?.Part.NoIndex ?? false;
            set => this.Part.NoIndex = value;
        }

        Dictionary<string, string> IOptions.ReplaceName => this.GetMerged(part => part?.ReplaceName);
        public FormattingOptions Formatting { get; }

        ILanguage IOptions.Language
        {
            get => this.GetValue(part => part?.Language);
            set => this.Part.Language = value;
        }

        public OptionsSet(OptionsSet parent, OptionsSet global, OptionsSet caller = null, object target = null)
            : base(parent, global, caller, target)
        {
            OptionsSetDebugger.Add(this);
            this.Formatting = new FormattingOptions(
                () => caller?.Formatting,
                () => global?.Formatting,
                () => parent?.Formatting,
                () => this.CastTo<IOptions>().Language?.Formatting
            );
        }
    }
}
