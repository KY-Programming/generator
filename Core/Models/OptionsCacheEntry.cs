using System;
using System.Collections.Generic;
using System.Linq;

namespace KY.Generator.Models
{
    public class OptionsCacheEntry
    {
        public List<Attribute> Attributes { get; set; } = new();
        public List<IOptions> Options { get; } = new();

        public OptionsCacheEntry(IEnumerable<Attribute> attributes)
        {
            this.Attributes.AddRange(attributes);
        }

        public OptionsCacheEntry Add(IOptions options)
        {
            this.Options.Add(options);
            return this;
        }

        public T Get<T>()
        {
            return this.Options.OfType<T>().FirstOrDefault();
        }
    }
}