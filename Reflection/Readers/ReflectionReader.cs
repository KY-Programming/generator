﻿using KY.Core;
using KY.Generator.Reflection.Configurations;
using KY.Generator.Transfer;
using KY.Generator.Transfer.Readers;

namespace KY.Generator.Reflection.Readers;

internal class ReflectionReader : ITransferReader
{
    private readonly ReflectionModelReader modelReader;
    private readonly Options options;

    public ReflectionReader(ReflectionModelReader modelReader, Options options)
    {
        this.modelReader = modelReader;
        this.options = options;
    }

    public void Read(ReflectionReadConfiguration configuration, GeneratorOptions? caller = null)
    {
        Type type = GeneratorTypeLoader.Get(configuration.Assembly, configuration.Namespace, configuration.Name);
        if (type == null)
        {
            Logger.Trace($"Class {configuration.Namespace}.{configuration.Name} not found");
            return;
        }
        try
        {
            ModelTransferObject selfModel = this.modelReader.Read(type, caller);
            GeneratorOptions modelOptions = this.options.Get<GeneratorOptions>(selfModel);
            if (configuration.OnlySubTypes || modelOptions.OnlySubTypes)
            {
                modelOptions.OnlySubTypes = true;
            }
        }
        catch
        {
            Logger.Warning("Reflection reader could not read " + type.FullName);
            throw;
        }
    }
}
