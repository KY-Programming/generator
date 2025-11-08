using KY.Core.Dependency;
using KY.Generator.Licensing;
using KY.Generator.Transfer;

namespace KY.Generator;

public static class DependencyResolverExtension
{
    public static IDependencyResolver CloneForCommand(this IDependencyResolver resolver)
    {
        DependencyResolver newResolver = new(resolver);
        newResolver.Bind<Options>().ToSingleton();
        newResolver.Bind<List<ITransferObject>>().To([]);
        newResolver.Bind<ILicenseService>().To(resolver.Get<ILicenseService>());
        return newResolver;
    }
}
