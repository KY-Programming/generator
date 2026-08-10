using System.Collections.Generic;

namespace AspDotNetAnnotations.Models
{
    public class GenericResult<T>
    {
        public IEnumerable<T> Rows { get; }
        public List<string> Strings { get; set; } = [];

        public GenericResult(IEnumerable<T> rows)
        {
            this.Rows = rows;
        }
    }
}
