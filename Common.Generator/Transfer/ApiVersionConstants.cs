namespace KY.Generator.Transfer;

public static class ApiVersionConstants
{
    /// <summary>
    /// The api version placeholder in an ASP.NET route template.
    /// </summary>
    public const string RouteToken = "{version:apiVersion}";

    /// <summary>
    /// The version a service is served under if it declares none. Matches ApiVersion.Default of Asp.Versioning, which
    /// a host can replace, but which can not be read from the assembly the service is generated from.
    /// </summary>
    public const string Default = "1.0";
}
