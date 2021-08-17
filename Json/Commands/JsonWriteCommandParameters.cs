using KY.Generator.Command;

namespace KY.Generator.Json.Commands
{
    public class JsonWriteCommandParameters : GeneratorCommandParameters
    {
        public string ModelName { get; set; }
        public string ModelNamespace { get; set; }
        public bool WithReader { get; set; }

        public JsonWriteCommandParameters()
        {
            this.FieldsToProperties = true;
            this.PropertiesToFields = false;
            this.WithReader = true;
        }
    }
}
