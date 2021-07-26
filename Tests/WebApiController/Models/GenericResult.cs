using System.Collections.Generic;

namespace WebApiController.Models
{
    public class GenericResult<T>
    {
        public IEnumerable<T> Rows { get; }

        public GenericResult(IEnumerable<T> rows)
        {
            this.Rows = rows;
        }
    }
}
