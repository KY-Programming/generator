namespace KY.Generator.Angular.Fluent
{
    public interface IAngularWriteSyntax
    {
        IAngularModelOrAngularWriteSyntax AngularModel();
        IAngularServiceOrAngularWriteSyntax AngularServices();
    }
}