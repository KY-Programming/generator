using KY.Generator.Languages;

namespace KY.Generator.Tsql.Language
{
    public class TsqlLanguage : EmptyLanguage
    {
        public static TsqlLanguage Instance { get; } = new TsqlLanguage();

        public override string Name => "Tsql";

        private TsqlLanguage()
        { }
    }
}