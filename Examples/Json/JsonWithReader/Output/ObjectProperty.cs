﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated with KY.Generator 7.4.0.0
//
//      Manual changes to this file may cause unexpected behavior in your application.
//      Manual changes to this file will be overwritten if the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
// ReSharper disable All

using System.CodeDom.Compiler;
using System.IO;
using Newtonsoft.Json;

[GeneratedCode("KY.Generator", "7.4.0.0")]
public partial class ObjectProperty
{
    public string Property { get; set; }

    public static ObjectProperty Load(string fileName)
    {
        return Parse(File.ReadAllText(fileName));
    }

    public static ObjectProperty Parse(string json)
    {
        return JsonConvert.DeserializeObject<ObjectProperty>(json);
    }
}

