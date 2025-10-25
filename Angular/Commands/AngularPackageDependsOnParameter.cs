namespace KY.Generator;

public class AngularPackageDependsOnParameter
{
    public string Name { get; }
    public string Version { get; }

    public AngularPackageDependsOnParameter(string name, string version)
    {
        this.Name = name;
        this.Version = version;
    }
}
