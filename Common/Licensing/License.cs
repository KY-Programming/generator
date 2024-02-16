using System;
using System.Collections.Generic;

namespace KY.Generator.Licensing;

public class License
{
    public Guid Id { get; set; }
    public DateTime ValidUntil { get; set; }
    public List<Message> Messages { get; set; } = new();
}
