using KY.Core.Dependency;
using KY.Core.Module;
using KY.Generator.Command;
using KY.Generator.Mappings;
using KY.Generator.OData.Commands;
using KY.Generator.OData.Configurations;
using KY.Generator.OData.Extensions;

namespace KY.Generator.OData
{
    public class ODataModule : ModuleBase
    {
        public ODataModule(IDependencyResolver dependencyResolver)
            : base(dependencyResolver)
        { }

        public override void Initialize()
        {
            this.DependencyResolver.Get<ITypeMapping>().Initialize();
            this.DependencyResolver.Get<CommandRegistry>()
                .Register<ReadODataCommand, ODataReadConfiguration>("odata-v4", "read");
        }
    }
}