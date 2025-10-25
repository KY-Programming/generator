namespace KY.Generator;

public class AngularHttpClientMethodOptions
{
    public bool HasHttpOptions { get; set; } = true;
    public bool NotGeneric { get; set; }
    public bool ReturnGeneric { get; set; } = true;
    public bool ParameterGeneric { get; set; }
    public bool ReturnsAny { get; set; }
}
