namespace KY.Generator;

public interface IAssemblyLocator
{
    AssemblyLocation? Locate(AssemblyLocateInfo info);
}
