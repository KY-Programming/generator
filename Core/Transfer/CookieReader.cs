using System.Collections.Generic;
using System.Net;
using KY.Generator.Configuration;
using KY.Generator.Configurations;

namespace KY.Generator.Transfer
{
    public class CookieReader : ITransferReader
    {
        public virtual void Read(ConfigurationBase configurationBase, List<ITransferObject> transferObjects)
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
        }

        private void ReadFromUrl(CookieConfiguration configuration, List<ITransferObject> list)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(configuration.Url);
            request.CookieContainer = new CookieContainer();
            request.Credentials = CredentialCache.DefaultCredentials;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
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