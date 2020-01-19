using System;
using System.Linq;
using System.Reflection;
using KY.Core;
using KY.Generator.Command;
using KY.Generator.Configuration;
using KY.Generator.Configurations;

namespace KY.Generator.Commands
{
    internal class VersionCommand : ICommandLineCommand
    {
        public bool Execute(IConfiguration configurationBase)
        {
            Logger.Trace("Execute version command...");
            AssemblyName callingName = Assembly.GetEntryAssembly()?.GetName();
            Logger.Trace($"{callingName?.Name} {callingName?.Version}");
            VersionConfiguration configuration = (VersionConfiguration)configurationBase;
            Logger.Trace("Loaded assemblies:");

            AppDomain.CurrentDomain.GetAssemblies()
                     .Select(x => x.GetName())
                     .OrderBy(x => x.Name)
                     .ForEach(x => Logger.Trace($"{x.Name} {x.Version} {(configuration.Detailed ? x.CodeBase.TrimStart("file:///") : "")}"));
            return true;
        }
    }
}