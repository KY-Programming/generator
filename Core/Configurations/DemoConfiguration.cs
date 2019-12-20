namespace KY.Generator.Configurations
{
    public class DemoConfiguration : ConfigurationBase
    {
        public string Message { get; set; }
        public override bool RequireLanguage => false;
    }
}