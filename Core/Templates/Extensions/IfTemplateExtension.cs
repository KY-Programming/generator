namespace KY.Generator.Templates.Extensions
{
    public static class IfTemplateExtension
    {
        public static IfTemplate WithCode(this IfTemplate template, ICodeFragment code)
        {
            template.Code.AddLine(code);
            return template;
        }

        public static ElseIfTemplate WithCode(this ElseIfTemplate template, ICodeFragment code)
        {
            template.Code.AddLine(code);
            return template;
        }

        public static ElseTemplate WithCode(this ElseTemplate template, ICodeFragment code)
        {
            template.Code.AddLine(code);
            return template;
        }

        public static ElseIfTemplate ElseIf(this IfTemplate template, ICodeFragment condition)
        {
            ElseIfTemplate elseIfTemplate = new ElseIfTemplate(template, condition);
            template.ElseIf.Add(elseIfTemplate);
            return elseIfTemplate;
        }

        public static ElseIfTemplate ElseIf(this ElseIfTemplate template, ICodeFragment condition)
        {
            ElseIfTemplate elseIfTemplate = new ElseIfTemplate(template.IfTemplate, condition);
            template.IfTemplate.ElseIf.Add(elseIfTemplate);
            return elseIfTemplate;
        }

        public static ElseTemplate Else(this IfTemplate template)
        {
            ElseTemplate elseTemplate = new ElseTemplate(template);
            template.Else = elseTemplate;
            return elseTemplate;
        }

        public static ElseTemplate Else(this ElseIfTemplate template)
        {
            ElseTemplate elseTemplate = new ElseTemplate(template.IfTemplate);
            template.IfTemplate.Else = elseTemplate;
            return elseTemplate;
        }
    }
}