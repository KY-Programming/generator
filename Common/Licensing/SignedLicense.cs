using System;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace KY.Generator.Licensing;

internal class SignedLicense
{
    public License License { get; set; }
    public string Signature { get; set; }
    
    public bool Validate()
    {
        if (this.License == null)
        {
            return false;
        }
        string json = JsonConvert.SerializeObject(this.License);
        byte[] jsonBytes = Encoding.UTF8.GetBytes(json);
        byte[] signatureBytes = Convert.FromBase64String(this.Signature);
        using RSACryptoServiceProvider rsa = new();
        rsa.FromXmlString(key);
        return rsa.VerifyData(jsonBytes, SHA256.Create(), signatureBytes) && DateTime.UtcNow < this.License.ValidUntil;
    }

    private const string key = "<RSAKeyValue><Modulus>tvvrK6PvkX16tJnks2ouA/SKZhAf/Kuu5rVpJFdCp/sCNmvYO+3/XJ75pRamMadGcRu0vV1QnDgnSKcZxeyeaXp3/i3cs2WWlh4yFZMpIL31XpVCjh1IDnH+XulK/kwyeratBGG+ck3ZzoojJaLvWsVDBE41pMjIm9ZjWsCgTsU=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
}
