namespace KY.Generator.Angular.Configurations
{
    internal class AngularServiceConfiguration
    {
        public string Name { get; set; }
        public string Namespace { get; set; }
        public string RelativePath { get; set; }
        public bool SkipNamespace { get; set; }
        public string ModelPath { get; set; }
        public bool PropertiesToFields { get; set; }
        public bool FieldsToProperties { get; set; }

        public AngularServiceConfiguration()
        {
            this.SkipNamespace = true;
        }
    }
}