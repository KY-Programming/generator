namespace KY.Generator.Output
{
    public interface IOutputAction
    {
        string FilePath { get; }
        void Execute();
    }
}