using System;

namespace KY.Generator.Output
{
    public interface IOutput
    {
        void Write(string fileName, string content, Guid? outputId);
        void Delete(string fileName);
        void DeleteAllRelatedFiles(Guid? outputId, string relativePath = null);
        void Execute();
        void Move(string relativePath);
    }
}