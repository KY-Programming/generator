using System.Collections.Generic;
using System.Net;
using KY.Core;
using KY.Generator.Command;
using KY.Generator.Configuration;
using KY.Generator.Configurations;
using KY.Generator.Transfer;

namespace KY.Generator.Commands
{
    public class ReadCookieCommand : IConfigurationCommand
    {
        public bool Execute(IConfiguration configurationBase, List<ITransferObject> transferObjects)
        {
            CookieConfiguration configuration = (CookieConfiguration)configurationBase;
            if (!string.IsNullOrEmpty(configuration.Url))
            {
                this.ReadFromUrl(configuration, transferObjects);
            }
            else if (!string.IsNullOrEmpty(configuration.Name))
            {
                transferObjects.Add(new TransferObject<Cookie>(new Cookie(configuration.Name, configuration.Value, configuration.Path, configuration.Domain)));
            }
            return true;
        }

        private void ReadFromUrl(CookieConfiguration configuration, List<ITransferObject> list)
        {
            Logger.Trace($"Read cookies from {configuration.Url}");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(configuration.Url);
            request.CookieContainer = new CookieContainer();
            request.Credentials = CredentialCache.DefaultCredentials;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                Logger.Trace($"{response.Cookies.Count} cookies found");
                foreach (Cookie cookie in response.Cookies)
                {
                    if (cookie.Name == configuration.Name)
                    {
                        list.Add(TransferObject.Create(cookie));
                    }
                }
            }
        }
    }
}