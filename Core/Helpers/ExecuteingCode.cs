using System;
using System.Collections.Generic;
using KY.Generator.Templates;

namespace KY.Generator
{
    public static partial class Code
    {
        public static BlankLineTemplate BlankLine()
        {
            return new BlankLineTemplate();
        }

        public static LocalVariableTemplate Local(string name)
        {
            return new LocalVariableTemplate(name);
        }

        public static ExecuteMethodTemplate Method(string name, params ICodeFragment[] parameters)
        {
            return new ExecuteMethodTemplate(name, parameters);
        }

        public static NewTemplate New(TypeTemplate type, params ICodeFragment[] parameters)
        {
            return new NewTemplate(type, parameters);
        }

        public static NewTemplate New(TypeTemplate type, IEnumerable<ICodeFragment> parameters)
        {
            return new NewTemplate(type, parameters);
        }

        public static ThisTemplate This()
        {
            return new ThisTemplate();
        }

        public static BaseTemplate Base()
        {
            return new BaseTemplate();
        }

        public static ReturnTemplate Return(ICodeFragment code)
        {
            return new ReturnTemplate(code);
        }

        public static DeclareTemplate Declare(TypeTemplate type, string name, ICodeFragment code)
        {
            return new DeclareTemplate(type, name, code);
        }

        public static TypeOfTemplate TypeOf(TypeTemplate type)
        {
            return new TypeOfTemplate(type);
        }

        public static CastTemplate Cast(TypeTemplate type, ICodeFragment code)
        {
            return new CastTemplate(type, code);
        }

        public static IfTemplate If(ICodeFragment condition, Action<IfTemplate> action = null)
        {
            IfTemplate template = new IfTemplate(condition);
            action?.Invoke(template);
            return template;
        }

        public static InlineIfTemplate InlineIf(ICodeFragment condition, ICodeFragment trueFragment, ICodeFragment falseFragment)
        {
            return new InlineIfTemplate(condition, trueFragment, falseFragment);
        }

        public static SwitchTemplate Switch(ICodeFragment expression)
        {
            return new SwitchTemplate(expression);
        }

        public static ThrowTemplate Throw(TypeTemplate type, params ICodeFragment[] parameters)
        {
            return new ThrowTemplate(type, parameters);
        }

        public static ThrowTemplate ThrowArgumentOutOfRange(string parameterName)
        {
            return new ThrowTemplate(Type(nameof(ArgumentOutOfRangeException)),
                                     String(parameterName),
                                     Local(parameterName),
                                     String($"{parameterName} was out of the range of valid values."));
        }

        public static LambdaTemplate Lambda(string parameterName, ICodeFragment code)
        {
            return new LambdaTemplate(parameterName, code);
        }
    }
}