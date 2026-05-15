using KY.Generator;

namespace EdgeCases;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class RouteAttribute : Attribute
{
    public string Template { get; }

    public RouteAttribute(string template)
    {
        this.Template = template;
    }
}

[AttributeUsage(AttributeTargets.Method)]
public class HttpPostAttribute : Attribute
{
    public string Template { get; }

    public HttpPostAttribute(string template)
    {
        this.Template = template;
    }
}

/// <summary>
/// Action parameters named <c>url</c> or <c>httpOptions</c> must not collide with the locally-generated
/// URL string variable or the generated <c>httpOptions</c> parameter inside the produced TypeScript
/// service method body.
/// </summary>
[GenerateAngularService("Output", "Output")]
[Route("api/[controller]")]
public class HttpServiceParameterNamesController
{
    [HttpPost("download")]
    public string Download(string url) => url;

    [HttpPost("with-http-options")]
    public string WithHttpOptions(string httpOptions) => httpOptions;
}
