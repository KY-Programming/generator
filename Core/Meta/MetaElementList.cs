using System;
using System.Collections.ObjectModel;
using KY.Core;

namespace KY.Generator.Meta
{
    public class MetaElementList : Collection<MetaElement>, IMetaElementList
    {
        private readonly MetaElement parent;
        private readonly bool useParentLevel;

        public MetaElementList(MetaElement parent = null, bool useParentLevel = false)
        {
            this.parent = parent;
            this.useParentLevel = useParentLevel;
        }

        protected override void InsertItem(int index, MetaElement item)
        {
            if (item.Parent != null)
            {
                throw new InvalidOperationException("Element already has an parent!");
            }
            item.Parent = this.parent;
            item.UseParentLevel = this.useParentLevel;
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

        protected override void SetItem(int index, MetaElement item)
        {
            this[index].Parent = null;
            item.Parent = this.parent;
            base.SetItem(index, item);
        }
    }
}