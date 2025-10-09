namespace KY.Generator.Output;

public interface IOutput
{
    void Write(string fileName, string content, GeneratorOptions options, bool ignoreOutputId = false, bool forceOverwrite = false);
    void Delete(string fileName);
    void DeleteAllRelatedFiles(string relativePath = null);
    void Execute();
    void Move(string relativePath);
    long Lines { get; }
}