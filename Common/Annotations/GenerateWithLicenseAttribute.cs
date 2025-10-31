namespace KY.Generator;

/// <summary>
/// Use a certificate of a license to use advanced features.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly, Inherited = false)]
public class GenerateWithLicenseAttribute : Attribute
{
    public string Certificate { get; }

    public GenerateWithLicenseAttribute(string certificate)
    {
        this.Certificate = certificate;
    }
}
