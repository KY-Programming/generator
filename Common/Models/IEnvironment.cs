using System;
using System.Collections.Generic;
using KY.Generator.Transfer;

namespace KY.Generator.Models
{
    public interface IEnvironment
    {
        Guid OutputId { get; set; }
        List<ITransferObject> TransferObjects { get; }
        string OutputPath { get; set; }
        string ApplicationData { get; }
    }
}
