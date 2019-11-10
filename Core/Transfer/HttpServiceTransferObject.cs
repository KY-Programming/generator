using System.Collections.Generic;
using KY.Generator.Languages;

namespace KY.Generator.Transfer
{
    // TODO: Only temporary location. Move to better location
    public class HttpServiceTransferObject : ITransferObject
    {
        public string Name { get; set; }
        public string Route { get; set; }
        public ILanguage Language { get; set; }

        public List<HttpServiceActionTransferObject> Actions { get; }

        public HttpServiceTransferObject()
        {
            this.Actions = new List<HttpServiceActionTransferObject>();
        }
    }

    public class HttpServiceActionTransferObject
    {
        public string Name { get; set; }
        public TypeTransferObject ReturnType { get; set; }
        public string Route { get; set; }
        public bool RequireBodyParameter { get; set; }
        public List<HttpServiceActionParameterTransferObject> Parameters { get; }
        public HttpServiceActionTypeTransferObject Type { get; set; }

        public HttpServiceActionTransferObject()
        {
            this.Parameters = new List<HttpServiceActionParameterTransferObject>();
        }
    }

    public class HttpServiceActionParameterTransferObject
    {
        public string Name { get; set; }
        public bool FromBody { get; set; }
        public bool AppendName { get; set; } = true;
        public bool Inline { get; set; }
        public int InlineIndex { get; set; }
        public TypeTransferObject Type { get; set; }
    }

    public enum HttpServiceActionTypeTransferObject
    {
        Get,
        Post,
        Put,
        Patch,
        Delete
    }

    public static class HttpServiceActionTypeTransferObjectExtension
    {
        public static bool IsBodyParameterRequired(this HttpServiceActionTypeTransferObject type)
        {
            return type == HttpServiceActionTypeTransferObject.Post || type == HttpServiceActionTypeTransferObject.Patch || type == HttpServiceActionTypeTransferObject.Put;
        }
    }
}