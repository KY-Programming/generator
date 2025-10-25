using KY.Generator.Transfer.Writers;

namespace KY.Generator.Reflection.Writers
{
    internal class ReflectionWriter : ITransferWriter
    {
        private readonly ModelWriter modelWriter;

        public ReflectionWriter(ModelWriter modelWriter)
        {
            this.modelWriter = modelWriter;
        }

        public void FormatNames()
        {
            this.modelWriter.FormatNames();
        }

        public void Write(string relativePath)
        {
            this.modelWriter.Write(relativePath);
        }
    }
}
