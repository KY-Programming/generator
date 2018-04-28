using System;
using System.Collections.ObjectModel;
using KY.Core;

namespace KY.Generator.Meta
{
    public class MetaFragmentList : Collection<MetaFragment>, IMetaFragmentList
    {
        private readonly MetaElement parent;

        public MetaFragmentList(MetaElement parent = null)
        {
            this.parent = parent;
        }

        protected override void InsertItem(int index, MetaFragment item)
        {
            if (item.Parent != null)
            {
                throw new InvalidOperationException("Element already has an parent!");
            }
            item.Parent = this.parent;
            base.InsertItem(index, item);
        }

        protected override void RemoveItem(int index)
        {
            this[index].Parent = null;
            base.RemoveItem(index);
        }

        protected override void ClearItems()
        {
            this.ForEach(x => x.Parent = null);
            base.ClearItems();
        }

        protected override void SetItem(int index, MetaFragment item)
        {
            this[index].Parent = null;
            item.Parent = this.parent;
            base.SetItem(index, item);
        }
    }
}