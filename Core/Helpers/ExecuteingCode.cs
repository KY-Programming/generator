using System;
using System.Collections.Generic;
using KY.Generator.Templates;

namespace KY.Generator
{
    public static partial class Code
    {
        public static LocalVariableTemplate Local(string name)
        {
            return new LocalVariableTemplate(name);
        }

        public static ExecuteMethodTemplate Method(string name, params CodeFragment[] parameters)
        {
            return new ExecuteMethodTemplate(name, parameters);
        }

        public static NewTemplate New(TypeTemplate type, params CodeFragment[] parameters)
        {
            return new NewTemplate(type, parameters);
        }

        public static NewTemplate New(TypeTemplate type, IEnumerable<CodeFragment> parameters)
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

        public static ReturnTemplate Return(CodeFragment code)
        {
            return new ReturnTemplate(code);
        }

        public static DeclareTemplate Declare(TypeTemplate type, string name, CodeFragment code)
        {
            return new DeclareTemplate(type, name, code);
        }

        public static TypeOfTemplate TypeOf(TypeTemplate type)
        {
            return new TypeOfTemplate(type);
        }

        public static CastTemplate Cast(TypeTemplate type, CodeFragment code)
        {
            return new CastTemplate(type, code);
        }

        public static SwitchTemplate Switch(CodeFragment expression)
        {
            return new SwitchTemplate(expression);
        }

        public static ThrowTemplate Throw(TypeTemplate type, params CodeFragment[] parameters)
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
    }
}