using System.Collections.Generic;

namespace KY.Generator.Command;

public interface IGeneratorCommand
{
    GeneratorCommandParameters Parameters { get; }
    List<CliCommandParameter> OriginalParameters { get; set; }

    bool Parse();
    void Prepare();
    void FollowUp();
    IGeneratorCommandResult Run();
}
