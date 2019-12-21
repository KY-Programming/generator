using System.Collections.Generic;

namespace KY.Generator.Output
{
    public class MemoryOutput : IOutput
    {
        public Dictionary<string, string> Files { get; }

        public MemoryOutput()
        {
            this.Files = new Dictionary<string, string>();
        }

        public void Write(string fileName, string content)
        {
            this.Files.Add(fileName, content);
        }

        public void Delete(string fileName)
        {
            this.Files.Remove(fileName);
        }

        public void Execute()
        { }

        public void Move(string relativePath)
        { }

        public override string ToString()
        {
            return "Memory";
        }
    }
}