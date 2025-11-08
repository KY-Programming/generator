using KY.Generator.Templates;

namespace KY.Generator.Mappings;

public interface ITypeMappingTypeOrToDetailsSyntax : ITypeMappingTypeSyntax
{
    ITypeMappingTypeOrToDetailsSyntax FromSystem();
    ITypeMappingTypeOrToDetailsSyntax Nullable();
    ITypeMappingTypeOrToDetailsSyntax Namespace(string nameSpace);
    ITypeMappingTypeOrToDetailsSyntax Default(ICodeFragment? code);
    ITypeMappingTypeOrToDetailsSyntax Default(ICodeFragment? notStrictDefaultCode, ICodeFragment? strictDefaultCode);
}
