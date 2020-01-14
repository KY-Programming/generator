namespace KY.Generator.Syntax
{
    public interface IGeneratorAfterRunSyntax
    {
        bool GetResult();
        void SetExitCode();
    }
}