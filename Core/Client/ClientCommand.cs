using System;
using System.Net.Http;
using KY.Core;
using KY.Core.DataAccess;
using KY.Generator.Command;
using KY.Generator.Output;

namespace KY.Generator.Client
{
    internal class ClientCommand : IGeneratorCommand
    {
        public string[] Names { get; } = { "client" };

        public bool Generate(CommandConfiguration configuration, IOutput output)
        {
            string outputPath = configuration.Parameters.GetString("output");
            if (!string.IsNullOrEmpty(outputPath))
            {
                output = new FileOutput(outputPath);
            }
            if (output == null)
            {
                Logger.Error("No valid output specified");
                return false;
            }

            string url = configuration.Parameters.GetString("url");
            string launchSettings = configuration.Parameters.GetString("launchSettings");
            if (string.IsNullOrEmpty(url) && string.IsNullOrEmpty(launchSettings))
            {
                Logger.Error("No valid target found. Add at least a -url=... or a -launchSettings=... parameter");
                return false;
            }
            if (!string.IsNullOrEmpty(launchSettings))
            {
                LaunchSettingsReader reader = new LaunchSettingsReader();
                url = reader.ReadApplicationUrl(launchSettings);
                if (string.IsNullOrEmpty(url))
                {
                    Logger.Error("No value for iisSettings/iisExpress/applicationUrl in launchSettings.json found");
                    return false;
                }
            }

            string path = configuration.Parameters.GetString("path");
            string clientConfiguration = FileSystem.ReadAllText(PathResolver.Resolve(path, configuration));

            HttpClient client = new HttpClient();
            MultipartFormDataContent createContent = new MultipartFormDataContent();
            createContent.Add(new StringContent(clientConfiguration), "configuration");
            Logger.Trace($"Request {url}/generator/create");
            HttpResponseMessage createResponse = client.PostAsync($"{url}/generator/create", createContent).Result;
            if (!createResponse.IsSuccessStatusCode)
            {
                Logger.Error($"Connection to generator failed: {createResponse.StatusCode}");
                return false;
            }
            string id = createResponse.Content.ReadAsStringAsync().Result;
            MultipartFormDataContent getFilesContent = new MultipartFormDataContent();
            getFilesContent.Add(new StringContent(id), "id");
            Logger.Trace($"Request {url}/generator/getFiles");
            HttpResponseMessage getFilesResponse = client.PostAsync($"{url}/generator/getFiles", getFilesContent).Result;
            if (!getFilesResponse.IsSuccessStatusCode)
            {
                Logger.Error($"Connection to generator failed: {getFilesResponse.StatusCode}");
                return false;
            }
            string[] filePaths = getFilesResponse.Content.ReadAsStringAsync().Result.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            if (filePaths.Length == 0)
            {
                Logger.Warning("No files generated");
            }
            foreach (string filePath in filePaths)
            {
                MultipartFormDataContent getFileContent = new MultipartFormDataContent();
                getFileContent.Add(new StringContent(id), "id");
                getFileContent.Add(new StringContent(filePath), "path");
                Logger.Trace($"Request {url}/generator/getFile");
                HttpResponseMessage getFileResponse = client.PostAsync($"{url}/generator/getFile", getFileContent).Result;
                output.Write(filePath, getFileResponse.Content.ReadAsStringAsync().Result);
            }
            return true;
        }
    }
}