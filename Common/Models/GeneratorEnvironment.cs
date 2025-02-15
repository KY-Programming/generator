using System;
using System.Collections.Generic;
using KY.Core.DataAccess;
using KY.Generator.Command;
using KY.Generator.Transfer;

namespace KY.Generator.Models;

public class GeneratorEnvironment : IEnvironment
{
    public Guid OutputId { get; set; }
    public string Name { get; set; }
    public string OutputPath { get; set; }
    public List<ITransferObject> TransferObjects { get; } = [];
    public string ApplicationData { get; } = FileSystem.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "KY-Programming", "KY-Generator");
    public string LocalApplicationData { get; } = FileSystem.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "KY-Programming", "KY-Generator");
    public List<CliCommandParameter> Parameters { get; } = [];
}