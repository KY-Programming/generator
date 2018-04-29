namespace KY.Generator.Templates
{
    public class CsharpTemplate : CodeFragment
    {
        public string Code { get; set; }

        public CsharpTemplate(string code)
        {
            this.Code = code;
        }
    }
}