namespace KY.Generator.AspDotNet
{
    internal class AspDotNetControllerConfigureModule
    {
        public string Module { get; set; }
        public string Action { get; set; }

        public AspDotNetControllerConfigureModule()
        { }

        public AspDotNetControllerConfigureModule(string module, string action)
        {
            this.Module = module;
            this.Action = action;
        }
    }
}