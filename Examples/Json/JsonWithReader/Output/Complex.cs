﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated with KY.Generator 7.3.0.0
//
//      Manual changes to this file may cause unexpected behavior in your application.
//      Manual changes to this file will be overwritten if the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
// ReSharper disable All

using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

[GeneratedCode("KY.Generator", "7.3.0.0")]
public partial class Complex
{
    public List<string> StringArrayProperty { get; set; }
    public List<int> NumberArrayProperty { get; set; }
    public List<ObjectArrayProperty> ObjectArrayProperty { get; set; }
    public List<object> MixedArrayProperty { get; set; }
    public ObjectProperty ObjectProperty { get; set; }

    public static Complex Load(string fileName)
    {
        return Parse(File.ReadAllText(fileName));
    }

    public static Complex Parse(string json)
    {
        return JsonConvert.DeserializeObject<Complex>(json);
    }
}

