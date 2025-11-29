namespace KY.Generator;

public class SystemAssemblyLoader : IAssemblyLoader
{
    public AssemblyLocation? Load(AssemblyLocateInfo info)
    {
        if ( /*!forceSearchOnDisk &&*/ info.Name.StartsWith("System.") || info.Name.StartsWith("Microsoft."))
        {
            // Assembly assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(info.FullName));
            // if (loadDependencies)
            // {
            //     this.ResolveDependencies(assembly);
            // }
            // return AssemblyLoadInfo.Success(assembly);
        }
        return null;
    }
}
