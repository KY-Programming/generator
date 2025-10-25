using KY.Core;
using KY.Generator.Templates;
using KY.Generator.Transfer;

namespace KY.Generator;

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

    public static LocalVariableTemplate Local(this Code _, PropertyTemplate type)
    {
        return new LocalVariableTemplate(type.Name);
    }

    public static LocalVariableTemplate Local(this Code _, DeclareTemplate type)
    {
        return new LocalVariableTemplate(type.Name);
    }

    public static LocalVariableTemplate Local(this Code _, ParameterTemplate type)
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

    public static NewTemplate New(this Code code, TypeTransferObject type, params ICodeFragment[] parameters)
    {
        return new NewTemplate(code.Type(type), parameters);
    }

    public static ThisTemplate This(this Code _)
    {
        return new ThisTemplate();
    }

    public static BaseTemplate Base(this Code _)
    {
        return new BaseTemplate();
    }

    public static ReturnTemplate Return(this Code _, ICodeFragment code = null)
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

    public static CastTemplate Cast(this Code _, TypeTemplate type)
    {
        return new CastTemplate(type);
    }

    public static IfTemplate If(this Code _, ICodeFragment condition, Action<IfTemplate> action = null)
    {
        IfTemplate template = new(condition);
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

    public static LambdaTemplate Lambda(this Code _, ICodeFragment code)
    {
        return new LambdaTemplate(new string[0], code);
    }

    public static LambdaTemplate Lambda(this Code _, string parameterName, ICodeFragment code)
    {
        return new LambdaTemplate(parameterName.Yield(), code);
    }

    public static LambdaTemplate Lambda(this Code _, IEnumerable<string> parameterNames, ICodeFragment code)
    {
        return new LambdaTemplate(parameterNames, code);
    }

    public static LambdaTemplate Lambda(this Code _, ParameterTemplate parameter, ICodeFragment code)
    {
        return new LambdaTemplate(parameter.Yield(), code);
    }

    public static LambdaTemplate Lambda(this Code _, IEnumerable<ParameterTemplate> parameters, ICodeFragment code)
    {
        return new LambdaTemplate(parameters, code);
    }

    public static NotTemplate Not(this Code _)
    {
        return new NotTemplate();
    }

    public static WhileTemplate While(this Code _, ICodeFragment condition, ICodeFragment code = null)
    {
        return new WhileTemplate(condition, code);
    }

    public static ParenthesisTemplate Parenthesis(this Code _, ICodeFragment code)
    {
        return new ParenthesisTemplate(code);
    }
}
