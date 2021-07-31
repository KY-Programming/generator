namespace KY.Generator.Angular.Fluent
{
    public interface IAngularServiceSyntax
    {
        /// <inheritdoc cref="IAngularModelSyntax.SkipHeader"/>
        IAngularServiceSyntax SkipHeader();

        /// <inheritdoc cref="IAngularModelSyntax.FormatNames"/>
        IAngularServiceSyntax FormatNames(bool value = true);

        /// <inheritdoc cref="IAngularModelSyntax.OutputPath"/>
        IAngularServiceSyntax OutputPath(string path);

        /// <inheritdoc cref="IAngularModelSyntax.Strict"/>
        IAngularServiceSyntax Strict(bool value = true);

        /// <summary>
        /// Renames the controller. Use {0} to inject the controller name without "Controller"
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IAngularServiceSyntax Name(string name);
    }
}
