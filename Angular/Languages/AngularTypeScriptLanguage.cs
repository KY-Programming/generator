using KY.Generator.TypeScript.Languages;

namespace KY.Generator.Angular.Languages
{
    internal class AngularTypeScriptLanguage : TypeScriptLanguage
    {
        public AngularTypeScriptLanguage()
        {
            this.Key = TypeScriptLanguage.Instance.Key;
        }

        public override string FormatFileName(string fileName, bool isInterface)
        {
            return base.FormatFileName(fileName, isInterface).Replace("-service.ts", ".service.ts");
        }
    }
}