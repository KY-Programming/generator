using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using KY.Core;
using KY.Generator.Transfer;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;

namespace KY.Generator.OpenApi.DataAccess
{
    public class OpenApiUrlReader
    {
        public void Read(string url, List<ITransferObject> transferObjects)
        {
            HttpWebRequest request = WebRequest.CreateHttp(url);
            request.CookieContainer = new CookieContainer();
            transferObjects.OfType<TransferObject<Cookie>>().ForEach(x => request.CookieContainer.Add(x.Value));
            WebResponse response = request.GetResponse();

            string json;
            using (Stream responseStream = response.GetResponseStream())
            using (StreamReader streamReader = new StreamReader(responseStream))
            {
                json = streamReader.ReadToEnd();
            }
            OpenApiStringReader reader = new OpenApiStringReader();
            OpenApiDocument document = reader.Read(json, out OpenApiDiagnostic diagnostic);
            diagnostic.Errors?.ForEach(error => Logger.Error(error.Message));
            
            transferObjects.Add(TransferObject.Create(document));
        }
    }
}