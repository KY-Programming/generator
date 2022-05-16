using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WebApiController.Models
{
    public class OptionalPropertiesModel
    {
        [Required]
        public string RequiredString { get; set; }

        [Required, DefaultValue("")]
        public string RequiredNotNullableString { get; set; }

        [Required]
        public int RequiredInt { get; set; }

        public string OptionalString { get; set; }
        public int OptionalInt { get; set; }
    }
}
