namespace KY.Generator.Angular.Fluent
{
    public interface IAngularServiceSyntax
    {
        /// <inheritdoc cref="IAngularModelSyntax.SkipHeader"/>
        IAngularServiceOrAngularWriteSyntax SkipHeader();
        
        /// <inheritdoc cref="IAngularModelSyntax.FormatNames"/>
        IAngularServiceOrAngularWriteSyntax FormatNames(bool value = true);
        
        /// <inheritdoc cref="IAngularModelSyntax.OutputPath"/>
        IAngularServiceOrAngularWriteSyntax OutputPath(string path);
    }
}