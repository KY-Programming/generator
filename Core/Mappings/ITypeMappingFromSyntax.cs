namespace KY.Generator.Mappings
{
    public interface ITypeMappingFromSyntax
    {
        ITypeMappingTypeOrToDetailsSyntax To(string type, string constructor = null);
    }
}
