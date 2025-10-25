using KY.Core;
using KY.Generator.Reflection.Configurations;
using KY.Generator.Transfer;
using KY.Generator.Transfer.Readers;

namespace KY.Generator.Reflection.Readers;

internal class ReflectionReader : ITransferReader
{
    private readonly ReflectionModelReader modelReader;
    private readonly Options options;
    private readonly GeneratorTypeLoader typeLoader;

    public ReflectionReader(ReflectionModelReader modelReader, Options options, GeneratorTypeLoader typeLoader)
    {
        this.modelReader = modelReader;
        this.options = options;
        this.typeLoader = typeLoader;
    }

    public void Read(ReflectionReadConfiguration configuration, GeneratorOptions? caller = null)
    {
        Type? type = this.typeLoader.Get(configuration.Namespace, configuration.Name);
        if (type == null)
        {
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
