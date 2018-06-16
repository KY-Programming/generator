namespace KY.Generator.AspDotNet
{
    internal class GeneratorConfigurationConfigureModule
    {
        public string Module { get; set; }
        public string Action { get; set; }

        public GeneratorConfigurationConfigureModule()
        { }

        public GeneratorConfigurationConfigureModule(string module, string action)
        {
            this.Module = module;
            this.Action = action;
        }
    }
}