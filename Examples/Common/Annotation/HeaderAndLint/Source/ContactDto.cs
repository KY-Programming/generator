using KY.Generator;

namespace HeaderAndLint.Source;

// The same defaults for the C# writer: no version in the header, and the comment of the language -
// // ReSharper disable All. The Dto suffix is dropped so the generated Contact can live next to this type.
[GenerateCsharpModel("Output/Models")]
[GenerateClass(Replace = "Dto")]
public class ContactDto
{
    public string Mail { get; set; } = string.Empty;
    public int Extension { get; set; }
}
