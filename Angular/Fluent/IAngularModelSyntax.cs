namespace KY.Generator.Angular.Fluent
{
    public interface IAngularModelSyntax
    {
        IAngularModelOrAngularWriteSyntax SkipHeader();
        IAngularModelOrAngularWriteSyntax FormatNames(bool value = true);
        IAngularModelOrAngularWriteSyntax OutputPath(string path);
        IAngularModelOrAngularWriteSyntax SkipNamespace(bool value = true);
        IAngularModelOrAngularWriteSyntax PropertiesToFields(bool value = true);
        IAngularModelOrAngularWriteSyntax FieldsToProperties(bool value = true);
    }
}