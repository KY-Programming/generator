namespace KY.Generator.Angular;

public interface IAngularHttpClientMethodSyntax
{
    IAngularHttpClientMethodSyntax NoHttpOptions();
    IAngularHttpClientMethodSyntax NotGeneric();
    IAngularHttpClientMethodSyntax ReturnGeneric();
    IAngularHttpClientMethodSyntax ParameterGeneric();
    IAngularHttpClientMethodSyntax ReturnsAny();
}
