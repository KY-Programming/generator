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

        /// <summary>
        /// This property is generated, but the type itself will not be generated (e.g. to keep modifications). All used types will be generated
        /// </summary>
        public GenerateOnlySubTypes GenerateOnlySubTypes { get; set; }
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
