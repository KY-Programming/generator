using KY.Generator.Angular.Fluent;
using KY.Generator.Syntax;

// ReSharper disable once CheckNamespace
namespace KY.Generator
{
    public static class WriteFluentSyntaxExtension
    {
        /// <inheritdoc cref="IAngularWriteSyntax.AngularModel"/>
        public static IAngularModelOrAngularWriteSyntax AngularModels(this IWriteFluentSyntax syntax)
        {
            return new AngularWriteSyntax((IWriteFluentSyntaxInternal)syntax).AngularModel();
        }
        
        /// <inheritdoc cref="IAngularWriteSyntax.AngularServices"/>
        public static IAngularServiceOrAngularWriteSyntax AngularServices(this IWriteFluentSyntax syntax)
        {
            return new AngularWriteSyntax((IWriteFluentSyntaxInternal)syntax).AngularServices();
        }
    }
}