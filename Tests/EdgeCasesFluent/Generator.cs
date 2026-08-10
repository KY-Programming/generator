using KY.Generator;

namespace EdgeCasesFluent;

public class Generator : GeneratorFluentMain
{
    public override void Execute()
    {
        // The type graph edge cases of the reflection reader, driven from the fluent API: a generic and a
        // non generic base type of the same name in both orders, the same pair as interfaces, and types
        // whose base type or interface is ignored and must not contribute its member.
        this.SetType<IgnoreMe>(options => options.Ignore())
            .SetType<IgnoreMe<string>>(options => options.Ignore())
            .SetType<IIgnoreMe>(options => options.Ignore())
            .SetType<IIgnoreMe<string>>(options => options.Ignore())
            .Read(read => read.Reflection(reflection => reflection.FromType<TypeWithIgnoredBase>()
                                                                  .FromType<TypeWithGenericIgnoredBase>()
                                                                  .FromType<TypeWithIgnoreInterface>()
                                                                  .FromType<TypeWithIgnoreGenericInterface>()
                                                                  .FromType<TypeWithInterface>()
                                                                  .FromType<TypeWithGenericInterface>()
                                                                  .FromType<TypeWithGenericAndNotGenericInterface>()
                                                                  .FromType<TypeWithGenericAndNotGenericBaseInterface>()
                                                                  .FromType<EdgeCase1>()
                                                                  .FromType<EdgeCase2>()
                                                                  .FromType<SelfReferencingType>()))
            .Write(write => write.NoHeader()
                                 .NoIndex()
                                 .Angular(angular => angular.Models(config => config.OutputPath("Output"))));
    }
}
