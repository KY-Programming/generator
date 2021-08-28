using System.Collections.Generic;
using KY.Core.Dependency;
using KY.Generator.Transfer;

namespace KY.Generator.Extensions
{
    public static class DependencyResolverExtension
    {
        public static IDependencyResolver CloneForCommands(this IDependencyResolver resolver)
        {
            DependencyResolver newResolver = new(resolver);
            Options.Bind(newResolver);
            newResolver.Bind<IOptions>().To(newResolver.Get<Options>().Current);
            newResolver.Bind<List<ITransferObject>>().To(new List<ITransferObject>());
            return newResolver;
        }
    }
}
