namespace KY.Generator.Transfer
{
    public static class HttpServiceActionTypeTransferObjectExtension
    {
        public static bool IsBodyParameterRequired(this HttpServiceActionTypeTransferObject type)
        {
            return type == HttpServiceActionTypeTransferObject.Post || type == HttpServiceActionTypeTransferObject.Patch || type == HttpServiceActionTypeTransferObject.Put;
        }
    }
}