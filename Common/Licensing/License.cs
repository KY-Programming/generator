using System;
using System.Collections.Generic;

namespace KY.Generator.Licensing;

public class License
{
    public DateTime ValidUntil { get; set; }
    public List<Message> Messages { get; set; } = new();
}
