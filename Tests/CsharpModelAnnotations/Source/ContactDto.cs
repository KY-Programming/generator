using KY.Generator;

namespace CsharpModelAnnotations.Source;

// Reached as a sub type of CustomerDto and SupplierDto - never decorated with GenerateCsharpModel itself.
// The Dto suffix is dropped in the output so the generated Contact can live next to this type.
[GenerateClass(Replace = "Dto")]
public class ContactDto
{
    public string Mail { get; set; } = string.Empty;
    public int Extension { get; set; }
}
