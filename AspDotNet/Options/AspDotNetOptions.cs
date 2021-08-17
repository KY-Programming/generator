using System;
using System.Collections.Generic;

namespace KY.Generator.AspDotNet
{
    public class AspDotNetOptions : OptionsBase<AspDotNetOptionsSet, IAspDotNetOptions>
    {
        private readonly AspDotNetOptionsReader reader;
        private static Dictionary<object, AspDotNetOptionsSet> GlobalCache { get; } = new();
        private Dictionary<object, AspDotNetOptionsSet> Cache { get; } = new();

        public AspDotNetOptions(AspDotNetOptionsReader reader)
        {
            this.reader = reader;
        }

        protected override AspDotNetOptionsSet GetGlobalInstance() => AspDotNetOptionsSet.GlobalInstance;
        protected override Dictionary<object, AspDotNetOptionsSet> GetCache() => this.Cache;
        protected override Dictionary<object, AspDotNetOptionsSet> GetGlobalCache() => GlobalCache;
        protected override AspDotNetOptionsSet CreateSet(object key, AspDotNetOptionsSet parent, AspDotNetOptionsSet global, AspDotNetOptionsSet caller)
        {
            return new(parent, global, caller ?? (key is Type ? this.CurrentSet : null), key);
        }

        protected override AspDotNetOptionsSet CreateSetGlobal(object key, AspDotNetOptionsSet parent, AspDotNetOptionsSet caller)
        {
            AspDotNetOptionsSet entry = new(parent, null, caller, key);
            if (this.CanRead(key))
            {
                this.reader.Read(key, entry);
            }
            return entry;
        }
    }
}
