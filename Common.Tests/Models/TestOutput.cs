using System.Collections.Generic;
using System.Linq;
using KY.Generator.Output;

namespace KY.Generator.Common.Tests.Models;

public class TestOutput : IOutput
{
    public List<TestFile> Files { get; } = new();

    public long Lines { get; }

    public void Write(string fileName, string content, GeneratorOptions options, bool ignoreOutputId = false, bool forceOverwrite = false)
    {
        this.Files.Add(new TestFile(fileName, content));
    }

    public void Delete(string fileName)
    {
        this.Files.Remove(this.Files.FirstOrDefault(x => x.Name == fileName));
    }

    public void DeleteAllRelatedFiles(string relativePath = null)
    { }

    public void Execute()
    { }

    public void Move(string relativePath)
    { }
}

public class TestFile
{
    public string Name { get; }
    public string Content { get; }

    public TestFile(string name, string content)
    {
        this.Name = name;
        this.Content = content;
    }
}
