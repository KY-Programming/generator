using System;
using System.Collections.Generic;
using KY.Generator.Templates;

namespace KY.Generator
{
    public static class ExecutingCode
    {
        public static BlankLineTemplate BlankLine(this Code _)
        {
            return new BlankLineTemplate();
        }

        public static LocalVariableTemplate Local(this Code _, string name)
        {
            return new LocalVariableTemplate(name);
        }

        public static LocalVariableTemplate Local(this Code _, FieldTemplate type)
        {
            return new LocalVariableTemplate(type.Name);
        }

        public static LocalVariableTemplate Static(this Code _, TypeTemplate type)
        {
            return new LocalVariableTemplate(type.Name);
        }

        public static ExecuteMethodTemplate Method(this Code _, string name, params ICodeFragment[] parameters)
        {
            return new ExecuteMethodTemplate(name, parameters);
        }

        public static NewTemplate New(this Code _, TypeTemplate type, params ICodeFragment[] parameters)
        {
            return new NewTemplate(type, parameters);
        }

        public static NewTemplate New(this Code _, TypeTemplate type, IEnumerable<ICodeFragment> parameters)
        {
            return new NewTemplate(type, parameters);
        }

        public static ThisTemplate This(this Code _)
        {
            return new ThisTemplate();
        }

        public static BaseTemplate Base(this Code _)
        {
            return new BaseTemplate();
        }

        public static ReturnTemplate Return(this Code _, ICodeFragment code)
        {
            return new ReturnTemplate(code);
        }

        public static DeclareTemplate Declare(this Code _, TypeTemplate type, string name, ICodeFragment code)
        {
            return new DeclareTemplate(type, name, code);
        }

        public static TypeOfTemplate TypeOf(this Code _, TypeTemplate type)
        {
            return new TypeOfTemplate(type);
        }

        public static CastTemplate Cast(this Code _, TypeTemplate type, ICodeFragment code)
        {
            return new CastTemplate(type, code);
        }

        public static IfTemplate If(this Code _, ICodeFragment condition, Action<IfTemplate> action = null)
        {
            IfTemplate template = new IfTemplate(condition);
            action?.Invoke(template);
            return template;
        }

        public static InlineIfTemplate InlineIf(this Code _, ICodeFragment condition, ICodeFragment trueFragment, ICodeFragment falseFragment)
        {
            return new InlineIfTemplate(condition, trueFragment, falseFragment);
        }

        public static SwitchTemplate Switch(this Code _, ICodeFragment expression)
        {
            return new SwitchTemplate(expression);
        }

        public static ThrowTemplate Throw(this Code _, TypeTemplate type, params ICodeFragment[] parameters)
        {
            return new ThrowTemplate(type, parameters);
        }

        public static ThrowTemplate ThrowArgumentOutOfRange(this Code code, string parameterName)
        {
            return new ThrowTemplate(code.Type(nameof(ArgumentOutOfRangeException)),
                                     code.String(parameterName),
                                     code.Local(parameterName),
                                     code.String($"{parameterName} was out of the range of valid values."));
        }

        public static LambdaTemplate Lambda(this Code _, string parameterName, ICodeFragment code)
        {
            return new LambdaTemplate(parameterName, code);
        }
    }
}