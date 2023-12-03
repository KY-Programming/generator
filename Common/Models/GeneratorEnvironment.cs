using System;
using System.Collections.Generic;
using KY.Core.DataAccess;
using KY.Generator.Transfer;

namespace KY.Generator.Models
{
    public class GeneratorEnvironment : IEnvironment
    {
        public Guid OutputId { get; set; }
        public string Name { get; set; }
        public string ProjectFile { get; set; }
        public string OutputPath { get; set; }
        public List<ITransferObject> TransferObjects { get; } = new();
        public string ApplicationData { get; } = FileSystem.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "KY-Programming", "KY-Generator");
    }
}
