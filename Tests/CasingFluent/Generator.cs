using KY.Generator;

namespace CasingFluent;

public class Generator : GeneratorFluentMain
{
    public override void Execute()
    {
        // FormatNames(false) is set on one type only, so the same run has to case one model and leave
        // the other one alone.
        this.Read(read => read.Reflection(reflection => reflection.FromType<MixedCasing>()))
            .SetType<KeepMyCase>(config => config.FormatNames(false))
            .Write(write => write.NoHeader()
                                 .NoIndex()
                                 .Angular(angular => angular.Models(config => config.OutputPath("Output"))));
    }
}
