using KY.Generator.Transfer;

namespace KY.Generator.Json.Transfers
{
    public class JsonModelTransferObject : ModelTransferObject
    {
        /// <summary>
        /// True for the model that represents the document itself. Nested objects and object arrays
        /// become models of their own, but only the root model is named after the JSON file.
        /// </summary>
        public bool IsRoot { get; set; }
    }
}
