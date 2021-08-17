namespace KY.Generator.Angular.Configurations
{
    public class AngularWriteConfiguration
    {
        public AngularWriteServiceConfiguration Service { get; set; }
        public AngularWriteModelConfiguration Model { get; set; }
        public bool WriteModels { get; set; }

        public AngularWriteConfiguration()
        {
            this.WriteModels = true;
            this.Model = new AngularWriteModelConfiguration();
        }
    }
}
