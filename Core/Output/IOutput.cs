namespace KY.Generator.Output
{
    public interface IOutput
    {
        void Write(string fileName, string content);
        void Delete(string fileName);
        void Execute();
    }
}