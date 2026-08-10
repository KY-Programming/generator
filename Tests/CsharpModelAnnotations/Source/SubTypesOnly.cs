using KY.Generator;

namespace CsharpModelAnnotations.Source;

// onlySubTypes: only what this type holds reaches the output, the type itself does not.
[GenerateCsharpModel("Output", true)]
public class SubTypesOnly
{
    public SupplierDto? Supplier { get; set; }
}
