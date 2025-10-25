using System.Collections.Generic;
using KY.Core;
using KY.Generator.OpenApi.Configurations;
using KY.Generator.OpenApi.DataAccess;
using KY.Generator.Transfer;
using KY.Generator.Transfer.Readers;

namespace KY.Generator.OpenApi.Readers
{
    public class OpenApiReader : ITransferReader
    {
        private readonly OpenApiFileReader fileReader;
        private readonly OpenApiUrlReader urlReader;
        private readonly OpenApiDocumentReader documentReader;

        public OpenApiReader(OpenApiFileReader fileReader, OpenApiUrlReader urlReader, OpenApiDocumentReader documentReader)
        {
            this.fileReader = fileReader;
            this.urlReader = urlReader;
            this.documentReader = documentReader;
        }

        public void Read(OpenApiReadConfiguration configuration)
        {
            Logger.Trace("Read OpenApi...");
            if (configuration.File != null)
            {
                this.fileReader.Read(configuration.File);
            }
            if (configuration.Url != null)
            {
                this.urlReader.Read(configuration.File);
            }
            this.documentReader.Read(configuration);
        }
    }
}
