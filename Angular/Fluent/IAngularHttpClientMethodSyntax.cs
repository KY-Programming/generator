namespace KY.Generator.Angular.Fluent
{
    public interface IAngularHttpClientMethodSyntax
    {
        IAngularHttpClientMethodSyntax NoHttpOptions();
        IAngularHttpClientMethodSyntax NotGeneric();
        IAngularHttpClientMethodSyntax ReturnGeneric();
        IAngularHttpClientMethodSyntax ParameterGeneric();
        IAngularHttpClientMethodSyntax UseParameters();
        IAngularHttpClientMethodSyntax ReturnsAny();
    }
}