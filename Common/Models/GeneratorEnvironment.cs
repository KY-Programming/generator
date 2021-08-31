using System;
using System.Collections.Generic;
using KY.Generator.Transfer;

namespace KY.Generator.Models
{
    public class GeneratorEnvironment : IEnvironment
    {
        public Guid OutputId { get; set; }
        public string OutputPath { get; set; }
        public List<ITransferObject> TransferObjects { get; } = new();
    }
}
