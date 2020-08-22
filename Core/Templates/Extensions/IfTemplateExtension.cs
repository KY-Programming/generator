﻿namespace KY.Generator.Templates.Extensions
{
    public static class IfTemplateExtension
    {
        public static IfTemplate WithCode(this IfTemplate template, ICodeFragment code)
        {
            template.Code.AddLine(code);
            return template;
        }

        public static ElseIfTemplate ElseIf(this IfTemplate template, ICodeFragment condition)
        {
            ElseIfTemplate elseIfTemplate = new ElseIfTemplate(condition);
            template.ElseIf.Add(elseIfTemplate);
            return elseIfTemplate;
        }

        public static ElseTemplate Else(this IfTemplate template)
        {
            ElseTemplate elseTemplate = new ElseTemplate();
            template.Else = elseTemplate;
            return elseTemplate;
        }
    }
}