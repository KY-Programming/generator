using System.Collections.Generic;
using KY.Generator.Configuration;
using KY.Generator.Languages;
using KY.Generator.Output;
using Newtonsoft.Json;

namespace KY.Generator.Reflection.Configurations
{
    internal class ReflectionConfiguration : IConfigurationWithLanguage, IModelConfiguration
    {
        public ReadReflectionConfiguration Read { get; }
        public WriteReflectionConfiguration Write { get; }

        public string Assembly
        {
            get => this.Read.Assembly;
            set => this.Read.Assembly = value;
        }

        public string Name
        {
            get => this.Read.Name;
            set => this.Read.Name = value;
        }

        public string Namespace
        {
            get => this.Read.Namespace;
            set => this.Read.Namespace = value;
        }

        public string RelativePath
        {
            get => this.Write.RelativePath;
            set => this.Write.RelativePath = value;
        }

        public bool AddHeader
        {
            get => this.Write.AddHeader;
            set => this.Write.AddHeader = value;
        }

        public bool SkipHeader
        {
            get => !this.AddHeader;
            set => this.AddHeader = !value;
        }

        public bool SkipSelf
        {
            get => this.Read.SkipSelf;
            set => this.Read.SkipSelf = value;
        }

        public bool SkipNamespace
        {
            get => this.Write.SkipNamespace;
            set => this.Write.SkipNamespace = value;
        }

        public List<string> Usings
        {
            get => this.Write.Usings;
            set => this.Write.Usings = value;
        }

        public ConfigurationFormatting Formatting => this.Write.Formatting;

        public IOutput Output
        {
            get => this.Write.Output;
            set => this.Write.Output = value;
        }

        public ConfigurationEnvironment Environment => this.Write.Environment;

        public bool BeforeBuild
        {
            get => this.Read.BeforeBuild;
            set
            {
                this.Read.BeforeBuild = value;
                this.Write.BeforeBuild = value;
            }
        }

        [JsonIgnore]
        [ConfigurationIgnore]
        public ILanguage Language
        {
            get => this.Write.Language;
            set => this.Write.Language = value;
        }

        [JsonProperty("Language")]
        [ConfigurationProperty("Language")]
        public string LanguageKey
        {
            get => this.Write.LanguageKey;
            set => this.Write.LanguageKey = value;
        }

        public bool FormatNames
        {
            get => this.Write.FormatNames;
            set => this.Write.FormatNames = value;
        }

        public string Using
        {
            get => this.Write.Using;
            set => this.Write.Using = value;
        }

        public bool FieldsToProperties
        {
            get => this.Formatting.FieldsToProperties;
            set => this.Formatting.FieldsToProperties = value;
        }

        public bool PropertiesToFields
        {
            get => this.Formatting.PropertiesToFields;
            set => this.Formatting.PropertiesToFields = value;
        }

        public ReflectionConfiguration()
        {
            this.Read = new ReadReflectionConfiguration();
            this.Write = new WriteReflectionConfiguration();
            this.Write.Environment = this.Read.Environment;
        }
    }
}