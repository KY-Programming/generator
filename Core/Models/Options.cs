using System.Collections.Generic;

namespace KY.Generator.Models
{
    public class Options : ICoreOptions
    {
        protected static Dictionary<object, IOptions> Cache { get; } = new();

        public static IOptions Global { get; } = new Options();

        public Dictionary<string, object> Records { get; } = new();
        public IOptions Parent { get; set; }

        public bool Strict => this.GetBoolIncludeParent(nameof(this.Strict)) ?? false;
        public bool PropertiesToFields => this.GetBoolIncludeParent(nameof(this.PropertiesToFields)) ?? false;
        public bool FieldsToProperties => this.GetBoolIncludeParent(nameof(this.FieldsToProperties)) ?? false;
        public bool PreferInterfaces => this.GetBoolIncludeParent(nameof(this.PreferInterfaces)) ?? false;
        public bool OptionalFields => this.GetBoolIncludeParent(nameof(this.OptionalFields)) ?? false;
        public bool OptionalProperties => this.GetBoolIncludeParent(nameof(this.OptionalProperties)) ?? false;
        public bool Ignore => this.GetBoolIncludeParent(nameof(this.Ignore)) ?? false;

        public T GetRecord<T>(string key)
        {
            return this.Records.ContainsKey(key) ? (T)this.Records[key] : default;
        }

        protected bool? GetBoolIncludeParent(string key)
        {
            return this.GetRecord<bool?>(key) ?? (this.Parent as ICoreOptions)?.GetRecord<bool?>(key);
        }
    }
}
