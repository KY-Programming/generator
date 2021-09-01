using KY.Generator;
using Types;

namespace Fluent.Generator
{
    public class GeneratorMain : GeneratorFluentMain
    {
        public override void Execute()
        {
            this.SetType<IgnoreMe>(options => options.Ignore())
                .SetType<IgnoreMe<string>>(options => options.Ignore())
                .SetType<IIgnoreMe>(options => options.Ignore())
                .SetType<IIgnoreMe<string>>(options => options.Ignore())
                .Read()
                .FromType<Types.Types>()
                .FromType<TypeWithIgnoredBase>()
                .FromType<TypeWithGenericIgnoredBase>()
                .FromType<TypeWithIgnoreInterface>()
                .FromType<TypeWithIgnoreGenericInterface>()
                .FromType<TypeWithInterface>()
                .FromType<TypeWithGenericInterface>()
                .FromType<TypeWithGenericAndNotGenericInterface>()
                .FromType<TypeWithGenericAndNotGenericBaseInterface>()
                .FromType<EdgeCase1>()
                .FromType<EdgeCase2>()
                .FromType<SelfReferencingType>()
                .Write()
                .NoHeader()
                .NoIndex()
                .Angular(angular => angular.Models(config => config.OutputPath("Output/Models")));
        }
    }
}
