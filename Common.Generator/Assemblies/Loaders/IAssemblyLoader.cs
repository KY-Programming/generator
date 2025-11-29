namespace KY.Generator;

public interface IAssemblyLoader
{
    AssemblyLocation? Load(AssemblyLocateInfo info);
}
