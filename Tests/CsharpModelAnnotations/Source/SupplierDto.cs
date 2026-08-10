using KY.Generator;

namespace CsharpModelAnnotations.Source;

// Reached as a sub type of SubTypesOnly - never decorated with GenerateCsharpModel itself.
[GenerateClass(Replace = "Dto")]
public class SupplierDto
{
    public string Company { get; set; } = string.Empty;
    public ContactDto? Contact { get; set; }
}
