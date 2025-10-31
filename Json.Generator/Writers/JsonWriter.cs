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

    public JsonWriter FormatNames()
    {
        this.resolver.Create<ObjectWriter>().FormatNames();
        return this;
    }

    public void Write(bool withReader)
    {
        this.resolver.Create<ObjectWriter>().Write(withReader);
    }
}
