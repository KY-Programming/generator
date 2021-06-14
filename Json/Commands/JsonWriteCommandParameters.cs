using KY.Generator.Command;

namespace KY.Generator.Json.Commands
{
    public class JsonWriteCommandParameters : GeneratorCommandParameters
    {
        public string ModelPath { get; set; }
        public string ModelName { get; set; }
        public string ModelNamespace { get; set; }
        public bool WithReader { get; set; }
        public string ReaderPath { get; set; }
        public string ReaderName { get; set; }
        public string ReaderNamespace { get; set; }

        public JsonWriteCommandParameters()
        {
            this.FieldsToProperties = true;
            this.PropertiesToFields = false;
        }
    }
}