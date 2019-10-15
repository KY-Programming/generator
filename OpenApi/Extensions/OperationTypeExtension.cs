using System;
using KY.Generator.Transfer;
using Microsoft.OpenApi.Models;

namespace KY.Generator.OpenApi.Extensions
{
    public static class OperationTypeExtension
    {
        public static HttpServiceActionTypeTransferObject ToActionType(this OperationType type)
        {
            switch (type)
            {
                case OperationType.Get:
                    return HttpServiceActionTypeTransferObject.Get;
                case OperationType.Put:
                    return HttpServiceActionTypeTransferObject.Put;
                case OperationType.Post:
                    return HttpServiceActionTypeTransferObject.Post;
                case OperationType.Delete:
                    return HttpServiceActionTypeTransferObject.Delete;
                case OperationType.Patch:
                    return HttpServiceActionTypeTransferObject.Patch;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}