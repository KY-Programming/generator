using System.Collections;
using System.Collections.Generic;

namespace KY.Generator.Meta
{
    public class MetaSeparatorFragmentList : IMetaFragmentList
    {
        private readonly IMetaFragmentList list;
        private readonly string separator;

        public int Count => this.list.Count;
        public bool IsReadOnly => false;

        public MetaFragment this[int index]
        {
            get => this.list[index];
            set => this.list[index] = value;
        }

        public MetaSeparatorFragmentList(IMetaFragmentList list, string separator)
        {
            this.list = list;
            this.separator = separator;
        }

        public IEnumerator<MetaFragment> GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Add(MetaFragment item)
        {
            this.Insert(this.Count, item);
        }

        public void Clear()
        {
            this.list.Clear();
        }

        public bool Contains(MetaFragment item)
        {
            return this.list.Contains(item);
        }

        public void CopyTo(MetaFragment[] array, int arrayIndex)
        {
            this.list.CopyTo(array, arrayIndex);
        }

        public bool Remove(MetaFragment item)
        {
            int index = this.IndexOf(item);
            if (index == -1)
            {
                return false;
            }
            this.RemoveAt(index);
            return true;
        }

        public int IndexOf(MetaFragment item)
        {
            return this.list.IndexOf(item);
        }

        public void Insert(int index, MetaFragment item)
        {
            if (!this.IsPreviousSeperator(index))
            {
                this.Insert(index, item);
                index++;
            }
            this.list.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            if (this.IsPreviousSeperator(index))
            {
                this.RemoveAt(index);
                index--;
            }
            this.list.RemoveAt(index);
        }

        private bool IsPreviousSeperator(int index)
        {
            if (index > 0)
            {
                MetaFragment previousFragment = this[index - 1];
                return previousFragment.Code != this.separator;
            }
            return false;
        }
    }
}