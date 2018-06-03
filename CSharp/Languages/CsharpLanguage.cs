using System.Collections.Generic;
using System.Text.RegularExpressions;
using KY.Generator.Csharp.Templates;
using KY.Generator.Csharp.Writers;
using KY.Generator.Languages;
using KY.Generator.Templates;

namespace KY.Generator.Csharp.Languages
{
    public class CsharpLanguage : BaseLanguage
    {
        public override string Name => "Csharp";

        public CsharpLanguage()
        {
            this.TemplateWriters[typeof(AttributeTemplate)] = new AttributeWriter(this);
            this.TemplateWriters[typeof(BaseTypeTemplate)] = new BaseTypeWriter(this);
            this.TemplateWriters[typeof(BaseTemplate)] = new BaseWriter();
            this.TemplateWriters[typeof(CastTemplate)] = new CastWriter(this);
            this.TemplateWriters[typeof(CommentTemplate)] = new CommentWriter();
            this.TemplateWriters[typeof(ConstructorTemplate)] = new ConstructorWriter(this);
            this.TemplateWriters[typeof(ConstraintTemplate)] = new ConstraintWriter(this);
            this.TemplateWriters[typeof(CsharpTemplate)] = new CsharpWriter();
            this.TemplateWriters[typeof(DeclareTemplate)] = new DeclareWriter(this);
            this.TemplateWriters[typeof(NullCoalescingOperatorTemplate)] = new NullCoalescingOperatorWriter(this);
            this.TemplateWriters[typeof(ParameterTemplate)] = new ParameterWriter(this);
            this.TemplateWriters[typeof(ThrowTemplate)] = new ThrowWriter(this);
            this.TemplateWriters[typeof(UsingTemplate)] = new UsingWriter();
        }

        public override string GetFileName(FileTemplate fileTemplate)
        {
            return fileTemplate.Name + ".cs";
        }

        public override string GetClassName(ClassTemplate classTemplate)
        {
            return classTemplate.Name == null ? null : Regex.Replace(classTemplate.Name, "[^A-z0-9_]", "_");
        }

        public override string GetClassName(EnumTemplate classTemplate)
        {
            return classTemplate.Name == null ? null : Regex.Replace(classTemplate.Name, "[^A-z0-9_]", "_");
        }

        public override string GetPropertyName(PropertyTemplate propertyTemplate)
        {
            return propertyTemplate.Name == null ? null : Regex.Replace(propertyTemplate.Name, "[^A-z0-9_]", "_");
        }

        public override string GetFieldName(FieldTemplate fieldTemplate)
        {
            return fieldTemplate.Name == null ? null : Regex.Replace(fieldTemplate.Name, "[^A-z0-9_]", "_");
        }

        public override string GetMethodName(MethodTemplate methodTemplate)
        {
            return methodTemplate.Name == null ? null : Regex.Replace(methodTemplate.Name, "[^A-z0-9_]", "_");
        }

        protected override IEnumerable<UsingTemplate> GetUsings(FileTemplate fileTemplate)
        {
            return fileTemplate.GetUsingsByNamespace();
        }
    }
}