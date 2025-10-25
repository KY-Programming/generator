namespace KY.Generator.Angular;

public class AngularHttpClientSyntax : IAngularHttpClientMethodSyntax
{
    private readonly AngularHttpClientMethodOptions options;

    public AngularHttpClientSyntax(AngularHttpClientMethodOptions options)
    {
        this.options = options;
    }

    public IAngularHttpClientMethodSyntax NoHttpOptions()
    {
        this.options.HasHttpOptions = false;
        return this;
    }

    public IAngularHttpClientMethodSyntax NotGeneric()
    {
        this.options.NotGeneric = true;
        return this;
    }

    public IAngularHttpClientMethodSyntax ReturnGeneric()
    {
        this.options.ReturnGeneric = true;
        return this;
    }

    public IAngularHttpClientMethodSyntax ParameterGeneric()
    {
        this.options.ParameterGeneric = true;
        return this;
    }

    public IAngularHttpClientMethodSyntax ReturnsAny()
    {
        this.options.ReturnsAny = true;
        return this;
    }
}
