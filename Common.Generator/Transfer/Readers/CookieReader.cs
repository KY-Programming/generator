using System.Collections.Generic;
using System.Net;
using KY.Core;
using KY.Generator.Configurations;

namespace KY.Generator.Transfer.Readers
{
    public class CookieReader : ITransferReader
    {
        public virtual void Read(CookieConfiguration configuration, List<ITransferObject> transferObjects)
        {
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
