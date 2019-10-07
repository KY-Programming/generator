using System;
using System.Collections.Generic;
using KY.Core.Dependency;
using KY.Generator.Configuration;

namespace KY.Generator.Transfer
{
    //public class TransferGeneratorMapping
    //{
    //    private readonly IDependencyResolver resolver;
    //    private readonly Dictionary<Type, Type> mapping = new Dictionary<Type, Type>();

    //    public TransferGeneratorMapping(IDependencyResolver resolver)
    //    {
    //        this.resolver = resolver;
    //    }

    //    public TransferGeneratorMapping Map<TConfiguration, TWriter>()
    //    {
    //        return this.Map(typeof(TConfiguration), typeof(TWriter));
    //    }

    //    public TransferGeneratorMapping Map(Type configuration, Type writer)
    //    {
    //        if (configuration == null)
    //        {
    //            throw new ArgumentNullException(nameof(configuration));
    //        }
    //        if (writer == null)
    //        {
    //            throw new ArgumentNullException(nameof(writer));
    //        }
    //        this.mapping.Add(configuration, writer);
    //        return this;
    //    }

    //    public Type Resolve(Type configuration)
    //    {
    //        if (configuration == null)
    //        {
    //            throw new ArgumentNullException(nameof(configuration));
    //        }
    //        return this.mapping[configuration];
    //    }

    //    public ITransferReader ResolveReader(ConfigurationBase configuration)
    //    {
    //        if (configuration == null)
    //        {
    //            throw new ArgumentNullException(nameof(configuration));
    //        }
    //        Type type = configuration.GetType();
    //        return this.mapping.ContainsKey(type) ? this.resolver.Create(this.mapping[type]) as ITransferReader : null;
    //    }

    //    public ITransferWriter ResolveWriter(ConfigurationBase configuration)
    //    {
    //        if (configuration == null)
    //        {
    //            throw new ArgumentNullException(nameof(configuration));
    //        }
    //        Type type = configuration.GetType();
    //        return this.mapping.ContainsKey(type) ? this.resolver.Create(this.mapping[type]) as ITransferWriter : null;
    //    }
    //}
}