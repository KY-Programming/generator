using KY.Generator.Json.Transfers;

namespace KY.Generator.Json.Extensions;

public static class JsonModelTransferObjectExtension
{
    public static JsonModelTransferObject SetRoot(this JsonModelTransferObject model, bool isRoot = true)
    {
        model.IsRoot = isRoot;
        return model;
    }
}
