using KY.Generator.Angular.Fluent;
using KY.Generator.Syntax;

// ReSharper disable once CheckNamespace
namespace KY.Generator
{
    public static class WriteFluentSyntaxExtension
    {
        public static IAngularModelOrAngularWriteSyntax AngularModels(this IWriteFluentSyntax syntax)
        {
            return new AngularWriteSyntax((FluentSyntax)syntax).AngularModel();
        }

        public static IAngularServiceOrAngularWriteSyntax AngularService(this IWriteFluentSyntax syntax)
        {
            return new AngularWriteSyntax((FluentSyntax)syntax).AngularServices();
        }
    }
}