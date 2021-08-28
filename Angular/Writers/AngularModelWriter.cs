using KY.Generator.TypeScript.Transfer;

namespace KY.Generator.Angular.Writers
{
    public class AngularModelWriter
    {
        private readonly TypeScriptModelWriter modelWriter;

        public AngularModelWriter(TypeScriptModelWriter modelWriter)
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
