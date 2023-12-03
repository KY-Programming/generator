using System;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace KY.Generator.Licensing;

class LicenseValidator
{
    private const string key = "<RSAKeyValue><Modulus>tvvrK6PvkX16tJnks2ouA/SKZhAf/Kuu5rVpJFdCp/sCNmvYO+3/XJ75pRamMadGcRu0vV1QnDgnSKcZxeyeaXp3/i3cs2WWlh4yFZMpIL31XpVCjh1IDnH+XulK/kwyeratBGG+ck3ZzoojJaLvWsVDBE41pMjIm9ZjWsCgTsU=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

    public bool Validate(SignedLicense license)
    {
        if (license.License == null)
        {
            return false;
        }
        string json = JsonConvert.SerializeObject(license.License);
        byte[] jsonBytes = Encoding.UTF8.GetBytes(json);
        byte[] signatureBytes = Convert.FromBase64String(license.Signature);
        using RSACryptoServiceProvider rsa = new();
        rsa.FromXmlString(key);
        return rsa.VerifyData(jsonBytes, SHA256.Create(), signatureBytes) && DateTime.UtcNow < license.License.ValidUntil;
    }
}