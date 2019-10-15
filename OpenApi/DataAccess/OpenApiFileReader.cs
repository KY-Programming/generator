using System.Collections.Generic;
using KY.Core;
using KY.Core.DataAccess;
using KY.Generator.Transfer;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;

namespace KY.Generator.OpenApi.DataAccess
{
    public class OpenApiFileReader
    {
        public void Read(string filename, List<ITransferObject> transferObjects)
        {
            if (!FileSystem.FileExists(filename))
            {
                return;
            }
            string json = FileSystem.ReadAllText(filename);
            OpenApiStringReader reader = new OpenApiStringReader();
            OpenApiDocument document = reader.Read(json, out OpenApiDiagnostic diagnostic);
            diagnostic.Errors?.ForEach(error => Logger.Error(error.Message));
            transferObjects.Add(TransferObject.Create(document));
        }
    }
}