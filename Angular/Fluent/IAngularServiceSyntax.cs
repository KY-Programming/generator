namespace KY.Generator.Angular;

public interface IAngularServiceSyntax
{
    /// <inheritdoc cref="IAngularModelSyntax.FormatNames"/>
    IAngularServiceSyntax FormatNames(bool value = true);

    /// <inheritdoc cref="IAngularModelSyntax.OutputPath"/>
    IAngularServiceSyntax OutputPath(string path);

    /// <summary>
    /// Renames the controller. Use {0} to inject the controller name without "Controller"
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    IAngularServiceSyntax Name(string name);

    /// <summary>
    /// Type of a custom http client. If not used, the angular http client is used
    /// </summary>
    /// <param name="type"></param>
    /// <param name="import"></param>
    /// <returns></returns>
    IAngularHttpClientSyntax HttpClient(string type, string import);
}
