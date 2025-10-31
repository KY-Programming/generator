using KY.Generator.Transfer.Writers;

namespace KY.Generator.Reflection.Writers;

internal class ReflectionWriter : ITransferWriter
{
    private readonly ModelWriter modelWriter;

    public ReflectionWriter(ModelWriter modelWriter)
    {
        this.modelWriter = modelWriter;
    }

    public ReflectionWriter FormatNames()
    {
        this.modelWriter.FormatNames();
        return this;
    }

    public void Write()
    {
        this.modelWriter.Write();
    }
}
