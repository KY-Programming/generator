using System;
using System.Linq;
using KY.Core.Meta;
using KY.Generator.Extensions;
using KY.Generator.Languages;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;

namespace KY.Generator.Writers
{
    public class TypeScriptClassWriter : ClassWriter
    {
        public TypeScriptClassWriter(BaseLanguage language)
            : base(language)
        { }

        public override void Write(IMetaFragmentList fragments, CodeFragment fragment)
        {
            ClassTemplate template = (ClassTemplate)fragment;
            if (template.IsStatic)
            {
                throw new InvalidOperationException("Static classes not yet supported");
            }
            if (!template.IsStatic && !template.Methods.OfType<ConstructorTemplate>().Any())
            {
                ConstructorTemplate constructorTemplate = (ConstructorTemplate)template.AddConstructor().WithParameter(Code.Generic("Partial", template.ToType()), "init?");
                if (template.BasedOn != null)
                {
                    constructorTemplate.WithCode(Code.Method("super", constructorTemplate.SuperParameters));
                }
                constructorTemplate.WithCode(Code.Local("Object").Method("assign").WithParameter(Code.This()).WithParameter(Code.Local("init")));
            }
        }
    }
}