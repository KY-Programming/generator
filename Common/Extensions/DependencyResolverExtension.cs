using System.Collections.Generic;
using KY.Core.Dependency;
using KY.Generator.Transfer;

namespace KY.Generator;

public static class DependencyResolverExtension
{
    public static IDependencyResolver CloneForCommand(this IDependencyResolver resolver)
    {
        DependencyResolver newResolver = new(resolver);
        newResolver.Bind<Options>().ToSingleton();
        newResolver.Bind<List<ITransferObject>>().To([]);
        return newResolver;
    }
}