﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KY.Generator.Json.Tests.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("KY.Generator.Json.Tests.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {
        ///  &quot;stringArrayProperty&quot;: [ &quot;One&quot;, &quot;Two&quot;, &quot;Three&quot; ],
        ///  &quot;numberArrayProperty&quot;: [ 1, 2, 3 ],
        ///  &quot;objectArrayProperty&quot;: [
        ///    { &quot;text&quot;: &quot;One&quot; },
        ///    { &quot;text&quot;: &quot;Two&quot; }
        ///  ],
        ///  &quot;mixedArrayProperty&quot;: [ 1, &quot;Two&quot;, 3 ],
        ///  &quot;objectProperty&quot;: {
        ///    &quot;property&quot;: &quot;works&quot;
        ///  }
        ///}.
        /// </summary>
        internal static string complex {
            get {
                return ResourceManager.GetString("complex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to // ReSharper disable All
        ///
        ///using System.Collections.Generic;
        ///
        ///namespace KY.Generator.Examples.Json
        ///{
        ///    public partial class Complex
        ///    {
        ///        public List&lt;string&gt; StringArrayProperty { get; set; }
        ///        public List&lt;int&gt; NumberArrayProperty { get; set; }
        ///        public List&lt;ObjectArrayProperty&gt; ObjectArrayProperty { get; set; }
        ///        public List&lt;object&gt; MixedArrayProperty { get; set; }
        ///        public ObjectProperty ObjectProperty { get; set; }
        ///    }
        ///}.
        /// </summary>
        internal static string Complex_cs {
            get {
                return ResourceManager.GetString("Complex_cs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {
        ///  &quot;version&quot;: 2,
        ///  &quot;generate&quot;: [
        ///    {
        ///      &quot;read&quot;: &quot;json&quot;,
        ///      &quot;source&quot;: &quot;Resources/complex.json&quot;
        ///    },
        ///    {
        ///      &quot;write&quot;: &quot;model&quot;,
        ///      &quot;language&quot;: &quot;csharp&quot;,
        ///      &quot;name&quot;: &quot;Complex&quot;,
        ///      &quot;namespace&quot;: &quot;KY.Generator.Examples.Json&quot;,
        ///      &quot;formatNames&quot;: true,
        ///      &quot;Map&quot;: {
        ///        &quot;Type&quot;: &quot;type&quot;,
        ///        &quot;From&quot;: &quot;ObjectProperty&quot;,
        ///        &quot;To&quot;: &quot;SubType&quot;
        ///      }
        ///    }
        ///  ]
        ///}.
        /// </summary>
        internal static string complex_generator {
            get {
                return ResourceManager.GetString("complex_generator", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {
        ///  &quot;version&quot;: 2,
        ///  &quot;generate&quot;: [
        ///    {
        ///      &quot;read&quot;: &quot;json&quot;,
        ///      &quot;source&quot;: &quot;Resources/complex.json&quot;
        ///    },
        ///    {
        ///      &quot;write&quot;: &quot;json&quot;,
        ///      &quot;language&quot;: &quot;csharp&quot;,
        ///      &quot;object&quot;: {
        ///        &quot;name&quot;: &quot;ComplexWithReader&quot;,
        ///        &quot;namespace&quot;: &quot;KY.Generator.Examples.Json&quot;,
        ///        &quot;formatNames&quot;: true,
        ///        &quot;withReader&quot;: true
        ///      }
        ///    }
        ///  ]
        ///}.
        /// </summary>
        internal static string complex_with_reader_generator {
            get {
                return ResourceManager.GetString("complex_with_reader_generator", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to // ReSharper disable All
        ///
        ///using System.Collections.Generic;
        ///using System.IO;
        ///using Newtonsoft.Json;
        ///
        ///namespace KY.Generator.Examples.Json
        ///{
        ///    public partial class ComplexWithReader
        ///    {
        ///        public List&lt;string&gt; StringArrayProperty { get; set; }
        ///        public List&lt;int&gt; NumberArrayProperty { get; set; }
        ///        public List&lt;ObjectArrayProperty&gt; ObjectArrayProperty { get; set; }
        ///        public List&lt;object&gt; MixedArrayProperty { get; set; }
        ///        public ObjectProperty ObjectProperty { get;  [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ComplexWithReader_cs {
            get {
                return ResourceManager.GetString("ComplexWithReader_cs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to // ReSharper disable All
        ///
        ///namespace KY.Generator.Examples.Json
        ///{
        ///    public partial class ObjectArrayProperty
        ///    {
        ///        public string Text { get; set; }
        ///    }
        ///}.
        /// </summary>
        internal static string ObjectArrayProperty_cs {
            get {
                return ResourceManager.GetString("ObjectArrayProperty_cs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to // ReSharper disable All
        ///
        ///namespace KY.Generator.Examples.Json
        ///{
        ///    public partial class ObjectProperty
        ///    {
        ///        public string Property { get; set; }
        ///    }
        ///}.
        /// </summary>
        internal static string ObjectProperty_cs {
            get {
                return ResourceManager.GetString("ObjectProperty_cs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {
        ///  &quot;stringProperty&quot;:  &quot;Test&quot;,
        ///  &quot;numberProperty&quot;:  123,
        ///  &quot;booleanProperty&quot;:  true 
        ///}.
        /// </summary>
        internal static string simple {
            get {
                return ResourceManager.GetString("simple", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to // ReSharper disable All
        ///
        ///namespace KY.Generator.Examples.Json
        ///{
        ///    public partial class Simple
        ///    {
        ///        public string StringProperty { get; set; }
        ///        public int NumberProperty { get; set; }
        ///        public bool BooleanProperty { get; set; }
        ///    }
        ///}.
        /// </summary>
        internal static string Simple_cs {
            get {
                return ResourceManager.GetString("Simple_cs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {
        ///  &quot;version&quot;: 2,
        ///  &quot;generate&quot;: [
        ///    {
        ///      &quot;read&quot;: &quot;json&quot;,
        ///      &quot;source&quot;: &quot;Resources/simple.json&quot;
        ///    },
        ///    {
        ///      &quot;write&quot;: &quot;json&quot;,
        ///      &quot;language&quot;: &quot;csharp&quot;,
        ///      &quot;object&quot;: {
        ///        &quot;name&quot;: &quot;SimpleWithoutReader&quot;,
        ///        &quot;namespace&quot;: &quot;KY.Generator.Examples.Json&quot;,
        ///        &quot;formatNames&quot;: true
        ///      }
        ///    }
        ///  ]
        ///}.
        /// </summary>
        internal static string simple_generator {
            get {
                return ResourceManager.GetString("simple_generator", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {
        ///  &quot;version&quot;: 2,
        ///  &quot;generate&quot;: [
        ///    {
        ///      &quot;read&quot;: &quot;json&quot;,
        ///      &quot;source&quot;: &quot;Resources/simple.json&quot;
        ///    },
        ///    {
        ///      &quot;write&quot;: &quot;json&quot;,
        ///      &quot;language&quot;: &quot;csharp&quot;,
        ///      &quot;object&quot;: {
        ///        &quot;name&quot;: &quot;SimpleWithReader&quot;,
        ///        &quot;namespace&quot;: &quot;KY.Generator.Examples.Json&quot;,
        ///        &quot;formatNames&quot;: true,
        ///        &quot;withReader&quot;: true
        ///      }
        ///    }
        ///  ]
        ///}.
        /// </summary>
        internal static string simple_with_reader_generator {
            get {
                return ResourceManager.GetString("simple_with_reader_generator", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {
        ///  &quot;version&quot;: 2,
        ///  &quot;generate&quot;: [
        ///    {
        ///      &quot;read&quot;: &quot;json&quot;,
        ///      &quot;source&quot;: &quot;Resources/simple.json&quot;
        ///    },
        ///    {
        ///      &quot;write&quot;: &quot;json&quot;,
        ///      &quot;language&quot;: &quot;csharp&quot;,
        ///      &quot;object&quot;: {
        ///        &quot;name&quot;: &quot;Simple&quot;,
        ///        &quot;namespace&quot;: &quot;KY.Generator.Examples.Json&quot;,
        ///        &quot;formatNames&quot;: true
        ///      },
        ///      &quot;reader&quot;:  {} 
        ///    }
        ///  ]
        ///}.
        /// </summary>
        internal static string simple_with_separate_reader_generator {
            get {
                return ResourceManager.GetString("simple_with_separate_reader_generator", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to // ReSharper disable All
        ///
        ///using System.IO;
        ///using Newtonsoft.Json;
        ///
        ///namespace KY.Generator.Examples.Json
        ///{
        ///    public partial class SimpleReader
        ///    {
        ///        public static Simple Load(string fileName)
        ///        {
        ///            return Parse(File.ReadAllText(fileName));
        ///        }
        ///
        ///        public static Simple Parse(string json)
        ///        {
        ///            return JsonConvert.DeserializeObject&lt;Simple&gt;(json);
        ///        }
        ///    }
        ///}.
        /// </summary>
        internal static string SimpleReader_cs {
            get {
                return ResourceManager.GetString("SimpleReader_cs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to // ReSharper disable All
        ///
        ///namespace KY.Generator.Examples.Json
        ///{
        ///    public partial class SimpleWithoutReader
        ///    {
        ///        public string StringProperty { get; set; }
        ///        public int NumberProperty { get; set; }
        ///        public bool BooleanProperty { get; set; }
        ///    }
        ///}.
        /// </summary>
        internal static string SimpleWithoutReader_cs {
            get {
                return ResourceManager.GetString("SimpleWithoutReader_cs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to // ReSharper disable All
        ///
        ///using System.IO;
        ///using Newtonsoft.Json;
        ///
        ///namespace KY.Generator.Examples.Json
        ///{
        ///    public partial class SimpleWithReader
        ///    {
        ///        public string StringProperty { get; set; }
        ///        public int NumberProperty { get; set; }
        ///        public bool BooleanProperty { get; set; }
        ///
        ///        public static SimpleWithReader Load(string fileName)
        ///        {
        ///            return Parse(File.ReadAllText(fileName));
        ///        }
        ///
        ///        public static SimpleWithReader Parse(string js [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string SimpleWithReader_cs {
            get {
                return ResourceManager.GetString("SimpleWithReader_cs", resourceCulture);
            }
        }
    }
}
