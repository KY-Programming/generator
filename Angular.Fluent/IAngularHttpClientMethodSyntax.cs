namespace KY.Generator;

public interface IAngularHttpClientMethodSyntax
{
    IAngularHttpClientMethodSyntax NoHttpOptions();
    IAngularHttpClientMethodSyntax NotGeneric();
    IAngularHttpClientMethodSyntax ReturnGeneric();
    IAngularHttpClientMethodSyntax ParameterGeneric();
    IAngularHttpClientMethodSyntax ReturnsAny();
}
