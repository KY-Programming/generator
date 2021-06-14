using System;
using KY.Core.Dependency;
using KY.Core.Dependency.Syntax;

namespace KY.Generator.Helpers
{
    public class DependencyResolverReference : IDependencyResolver
    {
        public IDependencyResolver Resolver { get; set; }

        public T TryGet<T>()
        {
            return this.Resolver.TryGet<T>();
        }

        public T Get<T>()
        {
            return this.Resolver.Get<T>();
        }

        public IBindToSyntax<T> Bind<T>()
        {
            return this.Resolver.Bind<T>();
        }

        public void Bind<TBind, TTo>() where TTo : TBind
        {
            this.Resolver.Bind<TBind, TTo>();
        }

        public void Bind<TBind, TTo>(TTo value) where TTo : TBind
        {
            this.Resolver.Bind<TBind, TTo>(value);
        }

        public void BindSingleton<TBind, TTo>() where TTo : TBind
        {
            this.Resolver.BindSingleton<TBind, TTo>();
        }

        public void Bind<TBind>(Func<TBind> function)
        {
            this.Resolver.Bind(function);
        }

        public object Get(Type type)
        {
            return this.Resolver.Get(type);
        }

        public T Create<T>(params object[] arguments)
        {
            return this.Resolver.Create<T>(arguments);
        }

        public object Create(Type type, params object[] arguments)
        {
            return this.Resolver.Create(type, arguments);
        }

        public bool Contains<T>()
        {
            return this.Resolver.Contains<T>();
        }

        public bool Contains(Type type)
        {
            return this.Resolver.Contains(type);
        }

        public T TryGet<T>(string name)
        {
            return this.Resolver.TryGet<T>(name);
        }

        public T Get<T>(string name)
        {
            return this.Resolver.Get<T>(name);
        }
    }
}