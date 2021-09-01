using System;
using System.Collections.Generic;
using System.Reflection;

namespace KY.Generator
{
    public abstract class OptionsBase<TSet, TInterface>
        where TSet : class, TInterface
        where TInterface : class
    {
        public TInterface Get(MemberInfo member, TInterface caller = null) => this.GetInternal(member, caller as TSet);
        public TInterface Get(ParameterInfo parameter, TInterface caller = null) => this.GetInternal(parameter, caller as TSet);
        public TInterface Get(Type type, TInterface caller = null) => this.GetInternal(type, caller as TSet);
        public TInterface Get(Assembly assembly, TInterface caller = null) => this.GetInternal(assembly, caller as TSet);

        public TInterface Get(object key)
        {
            if (this.GetCache().ContainsKey(key))
            {
                return this.GetCache()[key];
            }
            return this.GetCurrentInstance();
        }

        public bool Contains(object key)
        {
            return this.GetCache().ContainsKey(key);
        }

        public void Set(object key, TInterface options)
        {
            this.GetCache().Add(key, options as TSet);
        }

        private TSet GetGlobal(MemberInfo member, TSet caller = null)
        {
            return this.GetOrCreateGlobal(member, this.GetGlobal(member.DeclaringType), caller);
        }

        private TSet GetInternal(MemberInfo member, TSet caller = null)
        {
            return this.GetOrCreate(member, this.GetInternal(member.DeclaringType), this.GetGlobal(member), caller);
        }

        private TSet GetGlobal(ParameterInfo parameter, TSet caller = null)
        {
            return this.GetOrCreateGlobal(parameter, this.GetGlobal(parameter.Member), caller);
        }

        private TSet GetInternal(ParameterInfo parameter, TSet caller = null)
        {
            return this.GetOrCreate(parameter, this.GetInternal(parameter.Member), this.GetGlobal(parameter), caller);
        }

        private TSet GetGlobal(Type type, TSet caller = null)
        {
            return this.GetOrCreateGlobal(type, this.GetGlobal(type.Assembly), caller);
        }

        private TSet GetInternal(Type type, TSet caller = null)
        {
            return this.GetOrCreate(type, this.GetInternal(type.Assembly), this.GetGlobal(type), caller);
        }

        private TSet GetGlobal(Assembly assembly, TSet caller = null)
        {
            return this.GetOrCreateGlobal(assembly, null, caller);
        }

        private TSet GetInternal(Assembly assembly, TSet caller = null)
        {
            return this.GetOrCreate(assembly, this.GetCurrentInstance(), this.GetGlobal(assembly), caller);
        }

        private TSet GetOrCreateGlobal(object key, TSet parent, TSet caller = null)
        {
            TSet entry;
            Dictionary<object,TSet> cache = this.GetGlobalCache();
            if (cache.ContainsKey(key))
            {
                entry = cache[key];
            }
            else
            {
                entry = this.CreateSetGlobal(key, parent, caller);
                cache[key] = entry;
            }
            return entry;
        }

        protected TSet GetOrCreate(object key, TSet parent, TSet global, TSet caller = null)
        {
            TSet entry;
            Dictionary<object,TSet> cache = this.GetCache();
            if (cache.ContainsKey(key))
            {
                entry = cache[key];
            }
            else
            {
                entry = this.CreateSet(key, parent, global, caller);
                cache[key] = entry;
            }
            return entry;
        }

        protected bool CanRead(object target)
        {
            return !(target is Assembly assembly && assembly.FullName.StartsWith("System") || target is Type type && (type.Namespace?.StartsWith("System") ?? true));
        }

        protected abstract TSet GetCurrentInstance();
        protected abstract Dictionary<object, TSet> GetCache();
        protected abstract Dictionary<object, TSet> GetGlobalCache();
        protected abstract TSet CreateSet(object key, TSet parent, TSet global, TSet caller);
        protected abstract TSet CreateSetGlobal(object key, TSet parent, TSet caller);
    }
}
