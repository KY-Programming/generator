using System;
using System.Collections.Generic;
using KY.Core.Dependency;

namespace KY.Generator
{
    public class Options : OptionsBase<OptionsSet, IOptions>
    {
        private readonly List<IGlobalOptionsReader> globalReaders;
        private static List<Action<IDependencyResolver>> RegisteredOptions { get; } = new();
        private static Dictionary<object, OptionsSet> GlobalCache { get; } = new();
        private Dictionary<object, OptionsSet> Cache { get; } = new();

        public static IOptions Global => OptionsSet.GlobalInstance;
        public IOptions Current => this.CurrentSet;

        static Options()
        {
            Register<Options>();
        }

        public Options(List<IGlobalOptionsReader> globalReaders)
        {
            this.globalReaders = globalReaders;
        }

        public static void Register<T>()
        {
            RegisteredOptions.Add(resolver => resolver.Bind<T>().ToSingleton());
        }

        public static void Bind(IDependencyResolver resolver)
        {
            RegisteredOptions.ForEach(action => action(resolver));
        }

        protected override OptionsSet GetGlobalInstance() => OptionsSet.GlobalInstance;
        protected override Dictionary<object, OptionsSet> GetCache() => this.Cache;
        protected override Dictionary<object, OptionsSet> GetGlobalCache() => GlobalCache;

        protected override OptionsSet CreateSet(object key, OptionsSet parent, OptionsSet global, OptionsSet caller)
        {
            return new(parent, global, caller ?? (key is Type ? this.CurrentSet : null), key);
        }

        protected override OptionsSet CreateSetGlobal(object key, OptionsSet parent, OptionsSet caller)
        {
            OptionsSet entry = new(parent, null, caller, key);
            if (this.CanRead(key))
            {
                this.globalReaders.ForEach(reader => reader.Read(key, entry));
            }
            return entry;
        }
    }
}
