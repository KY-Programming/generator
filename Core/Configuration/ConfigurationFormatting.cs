namespace KY.Generator.Configuration
{
    public class ConfigurationFormatting
    {
        public string FileCase { get; set; }
        public string ClassCase { get; set; }
        public string FieldCase { get; set; }
        public string PropertyCase { get; set; }
        public string MethodCase { get; set; }
        public string ParameterCase { get; set; }
        public bool FieldsToProperties { get; set; }
        public bool PropertiesToFields { get; set; }
        public string AllowedSpecialCharacters { get; set; }

        public virtual void ApplyDefaults(ConfigurationFormatting defaults)
        {
            this.FileCase = this.FileCase ?? defaults.FileCase;
            this.ClassCase = this.ClassCase ?? defaults.ClassCase;
            this.FieldCase = this.FieldCase ?? defaults.FieldCase;
            this.PropertyCase = this.PropertyCase ?? defaults.PropertyCase;
            this.MethodCase = this.MethodCase ?? defaults.MethodCase;
            this.ParameterCase = this.ParameterCase ?? defaults.ParameterCase;
        }
    }
}