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
    }
}