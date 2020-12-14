namespace KY.Generator.Angular.Fluent
{
    public interface IAngularServiceSyntax
    {
        IAngularServiceOrAngularWriteSyntax SkipHeader();
        IAngularServiceOrAngularWriteSyntax FormatNames(bool value = true);
        IAngularServiceOrAngularWriteSyntax OutputPath(string path);

    }
}