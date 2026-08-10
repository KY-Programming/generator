using KY.Generator;

namespace CsharpModelAnnotations.Source;

// The plain case: the decorated type and the sub type it holds are both written below the relative path.
[GenerateCsharpModel("Output")]
[GenerateClass(Replace = "Dto")]
public class CustomerDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ContactDto? Contact { get; set; }
}
