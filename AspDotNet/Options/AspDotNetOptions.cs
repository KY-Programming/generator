namespace KY.Generator.AspDotNet;

public class AspDotNetOptions(AspDotNetOptions? parent, AspDotNetOptions? global, object? target = null)
    : OptionsBase<AspDotNetOptions>(parent, global, target)
{
    private bool? httpGet;
    private string? httpGetRoute;
    private bool? httpPost;
    private string? httpPostRoute;
    private bool? httpPatch;
    private string? httpPatchRoute;
    private bool? httpPut;
    private string? httpPutRoute;
    private bool? httpDelete;
    private string? httpDeleteRoute;
    private bool? isNonAction;
    private bool? isFromServices;
    private bool? isFromHeader;
    private bool? isFromBody;
    private bool? isFromQuery;
    private List<string>? apiVersion;
    private string? route;
    private Type? produces;
    private List<Type>? ignoreGenerics;
    private bool? fixCasingWithMapping;

    public bool HttpGet
    {
        get => this.GetOwnValue(x => x.httpGet);
        set => this.httpGet = value;
    }

    public string? HttpGetRoute
    {
        get => this.GetOwnValue(x => x.httpGetRoute);
        set => this.httpGetRoute = value;
    }

    public bool HttpPost
    {
        get => this.GetOwnValue(x => x.httpPost);
        set => this.httpPost = value;
    }

    public string? HttpPostRoute
    {
        get => this.GetOwnValue(x => x.httpPostRoute);
        set => this.httpPostRoute = value;
    }

    public bool HttpPatch
    {
        get => this.GetOwnValue(x => x.httpPatch);
        set => this.httpPatch = value;
    }

    public string? HttpPatchRoute
    {
        get => this.GetOwnValue(x => x.httpPatchRoute);
        set => this.httpPatchRoute = value;
    }

    public bool HttpPut
    {
        get => this.GetOwnValue(x => x.httpPut);
        set => this.httpPut = value;
    }

    public string? HttpPutRoute
    {
        get => this.GetOwnValue(x => x.httpPutRoute);
        set => this.httpPutRoute = value;
    }

    public bool HttpDelete
    {
        get => this.GetOwnValue(x => x.httpDelete);
        set => this.httpDelete = value;
    }

    public string? HttpDeleteRoute
    {
        get => this.GetOwnValue(x => x.httpDeleteRoute);
        set => this.httpDeleteRoute = value;
    }

    public bool IsNonAction
    {
        get => this.GetOwnValue(x => x.isNonAction);
        set => this.isNonAction = value;
    }

    public bool IsFromServices
    {
        get => this.GetOwnValue(x => x.isFromServices);
        set => this.isFromServices = value;
    }

    public bool IsFromHeader
    {
        get => this.GetOwnValue(x => x.isFromHeader);
        set => this.isFromHeader = value;
    }

    public bool IsFromBody
    {
        get => this.GetOwnValue(x => x.isFromBody);
        set => this.isFromBody = value;
    }

    public bool IsFromQuery
    {
        get => this.GetOwnValue(x => x.isFromQuery);
        set => this.isFromQuery = value;
    }

    public IReadOnlyList<string> ApiVersion => this.GetList(x => x.apiVersion);

    public string? Route
    {
        get => this.GetOwnValue(x => x.route);
        set => this.route = value;
    }

    public Type? Produces
    {
        get => this.GetOwnValue(x => x.produces);
        set => this.produces = value;
    }

    public IReadOnlyList<Type> IgnoreGenerics => this.GetList(x => x.ignoreGenerics);

    public bool FixCasingWithMapping
    {
        get => this.GetValue(x => x.fixCasingWithMapping);
        set => this.fixCasingWithMapping = value;
    }

    public void AddToApiVersion(params IEnumerable<string> versions)
    {
        this.apiVersion ??= [];
        this.apiVersion.AddRange(versions);
    }

    public void AddToIgnoreGenerics(params IEnumerable<Type> types)
    {
        this.ignoreGenerics ??= [];
        this.ignoreGenerics.AddRange(types);
    }
}
