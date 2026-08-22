namespace HeaderAndLint;

// The Dto suffix is dropped on write, so the generated Contact can live next to this type.
public class ContactDto
{
    public string Mail { get; set; } = string.Empty;
    public int Extension { get; set; }
}
