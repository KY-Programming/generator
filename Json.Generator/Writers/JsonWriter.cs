using KY.Core.Dependency;
using KY.Generator.Transfer.Writers;

namespace KY.Generator.Json.Writers;

internal class JsonWriter : ITransferWriter
{
    private readonly IDependencyResolver resolver;

    public JsonWriter(IDependencyResolver resolver)
    {
        this.resolver = resolver;
    }

    public JsonWriter SetModelInfo(string? name, string? nameSpace)
    {
        this.resolver.Create<ObjectWriter>().SetModelInfo(name, nameSpace);
        return this;
    }

    public JsonWriter FormatNames()
    {
        this.resolver.Create<ObjectWriter>().FormatNames();
        return this;
    }

    public void Write(bool withReader, string? relativePath)
    {
        this.resolver.Create<ObjectWriter>().Write(withReader, relativePath);
    }
}
