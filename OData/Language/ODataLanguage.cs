using KY.Generator.Languages;

namespace KY.Generator.OData.Language
{
    public class ODataLanguage : EmptyLanguage
    {
        public static ODataLanguage Instance { get; } = new ODataLanguage();

        public override string Name => "OData";

        private ODataLanguage()
        { }
    }
}