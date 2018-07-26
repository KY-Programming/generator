using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Xml.Linq;
using KY.Core.Xml;

namespace KY.Generator.Authentication
{
    internal class CookieReader
    {
        public IEnumerable<Cookie> Read(XElement configurationElement)
        {
            XElement cookiesElement = configurationElement.Element("Cookies");
            if (cookiesElement != null)
            {
                foreach (XElement cookieElement in cookiesElement.Elements())
                {
                    if (cookieElement.Name == "Read")
                    {
                        string from = cookieElement.GetStringAttribute("From");
                        HttpWebRequest request = WebRequest.CreateHttp(from);
                        request.Method = cookieElement.TryGetStringAttribute("Method", "POST");
                        request.ContentType = cookieElement.TryGetStringAttribute("ContentType", "application/x-www-form-urlencoded");

                        NameValueCollection parameters = HttpUtility.ParseQueryString(string.Empty);
                        XElement parametersElement = cookieElement.Element("Parameters");
                        if (parametersElement != null)
                        {
                            foreach (XElement parameterElement in parametersElement.Elements())
                            {
                                parameters.Add(parameterElement.Name.LocalName, parameterElement.Value);
                            }
                        }
                        byte[] data = Encoding.ASCII.GetBytes(parameters.ToString());
                        request.ContentLength = data.Length;

                        XElement headersElement = cookieElement.Element("Headers");
                        if (headersElement != null)
                        {
                            foreach (XElement headerElement in headersElement.Elements())
                            {
                                request.Headers.Add(headerElement.Name.LocalName, headerElement.Value);
                            }
                        }

                        using (Stream stream = request.GetRequestStream())
                        {
                            stream.Write(data, 0, data.Length);
                        }
                        request.CookieContainer = new CookieContainer();
                        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                        {
                            foreach (Cookie cookie in response.Cookies)
                            {
                                yield return cookie;
                            }
                        }
                    }
                    else
                    {
                        yield return new Cookie(cookieElement.Name.LocalName, cookieElement.Value, cookieElement.TryGetStringAttribute(nameof(Cookie.Path), "/"), cookieElement.TryGetStringAttribute(nameof(Cookie.Domain)));
                    }
                }
            }
        }
    }
}