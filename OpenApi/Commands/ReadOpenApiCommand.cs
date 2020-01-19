using System.Collections.Generic;
using KY.Core;
using KY.Generator.Command;
using KY.Generator.Configuration;
using KY.Generator.OpenApi.Configurations;
using KY.Generator.OpenApi.DataAccess;
using KY.Generator.OpenApi.Readers;
using KY.Generator.Transfer;

namespace KY.Generator.OpenApi.Commands
{
    public class ReadOpenApiCommand : IConfigurationCommand
    {
        private readonly OpenApiFileReader fileReader;
        private readonly OpenApiUrlReader urlReader;
        private readonly OpenApiDocumentReader documentReader;

        public ReadOpenApiCommand(OpenApiFileReader fileReader, OpenApiUrlReader urlReader, OpenApiDocumentReader documentReader)
        {
            this.fileReader = fileReader;
            this.urlReader = urlReader;
            this.documentReader = documentReader;
        }

        public bool Execute(IConfiguration configurationBase, List<ITransferObject> transferObjects)
        {
            Logger.Trace("Read OpenApi...");
            OpenApiReadConfiguration configuration = (OpenApiReadConfiguration)configurationBase;
            if (configuration.File != null)
            {
                this.fileReader.Read(configuration.File, transferObjects);
            }
            if (configuration.Url != null)
            {
                this.urlReader.Read(configuration.File, transferObjects);
            }
            this.documentReader.Read(configuration, transferObjects);
            return true;
        }
    }
}