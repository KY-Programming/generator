using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace KY.Generator
{
    public static class OptionsSetDebugger
    {
        private static List<OptionsSet> allOptions { get; } = new();

        [Conditional("DEBUG")]
        public static void Add(OptionsSet options)
        {
            allOptions.Add(options);
        }

        public static string Trace()
        {
            Entry root = new(null, null);
            foreach (OptionsSet options in allOptions)
            {
                if (options.Parent == null)
                {
                    root.Children.Add(new Entry(root, options));
                }
                else
                {
                    Add(root, options);
                }
            }
            StringBuilder builder = new();
            Trace(root, builder);
            return builder.ToString();
        }

        private static void Trace(Entry entry, StringBuilder builder)
        {
            builder.AppendLine($"{">>>>>>>>>>>>>>>>>".Substring(0, entry.Level)}{entry?.Target?.Target ?? "Root"}");
            foreach (Entry child in entry.Children)
            {
                Trace(child, builder);
            }
        }

        private static Entry Add(Entry entry, OptionsSet options)
        {
            Entry found = Find(entry, options);
            if (found != null)
            {
                return found;
            }
            if (entry.Target == options.Parent)
            {
                entry.Children.Add(new Entry(entry, options));
                return entry;
            }
            Entry parent = Add(entry, options.Parent);
            parent.Children.Add(new Entry(parent, options));
            return null;
        }

        private static Entry Find(Entry entry, OptionsSet options)
        {
            return entry.Target == options ? entry : entry.Children.Select(child => Find(child, options)).FirstOrDefault(found => found != null);
        }

        private class Entry
        {
            private readonly Entry parent;

            public int Level { get; }
            public OptionsSet Target { get; }
            public List<Entry> Children { get; } = new();

            public Entry(Entry parent, OptionsSet target)
            {
                this.parent = parent;
                this.Target = target;
                this.Level = parent == null ? 0 : parent.Level + 1;
            }
        }
    }
}