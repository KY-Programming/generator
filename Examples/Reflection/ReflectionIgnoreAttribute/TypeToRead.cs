using KY.Generator;

namespace ReflectionIgnoreAttribute
{
    [Generate(OutputLanguage.TypeScript, "Output")]
    public class TypeToRead
    {
        /// <summary>
        /// This field will be ignored
        /// </summary>
        [GenerateIgnore]
        public string StringFieldToIgnore;
        
        /// <summary>
        /// This field will be ignored
        /// </summary>
        [GenerateIgnore]
        public FieldTypeToIgnore FieldTypeFieldToIgnore;
        
        /// <summary>
        /// This field is written to output but the class not
        /// </summary>
        public TypeToIgnore TypeToIgnoreField;

        public string StringProperty { get; set; }
        public int NumberProperty { get; set; }
        
        /// <summary>
        /// This property will be ignored
        /// </summary>
        [GenerateIgnore]
        public string StringPropertyToIgnore { get; set; }

        /// <summary>
        /// This property will be ignored
        /// </summary>
        [GenerateIgnore]
        public PropertyTypeToIgnore PropertyTypeToIgnore { get; set; }

        /// <summary>
        /// This property is written to output but the class not
        /// </summary>
        public TypeToIgnore TypeToIgnoreProperty { get; set; }

        /// <summary>
        /// This property is written to output but the class not
        /// </summary>
        public EnumToIgnore EnumToIgnoreProperty { get; set; }
    }

    [GenerateIgnore]
    public class TypeToIgnore
    {
        public string StringProperty { get; set; }
    }

    [GenerateIgnore]
    public enum EnumToIgnore
    {
        None,
        Any
    }

    public class FieldTypeToIgnore
    {
        public string StringProperty { get; set; }
    }

    public class PropertyTypeToIgnore
    {
        public string StringProperty { get; set; }
    }
}