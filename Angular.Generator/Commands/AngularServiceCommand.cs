using KY.Core.Dependency;
using KY.Generator.Angular.Configurations;
using KY.Generator.Angular.Languages;
using KY.Generator.Angular.Writers;
using KY.Generator.Command;
using KY.Generator.Command.Extensions;
using KY.Generator.Output;
using KY.Generator.TypeScript;
using KY.Generator.TypeScript.Transfer;

namespace KY.Generator.Angular.Commands;

internal class AngularServiceCommand : GeneratorCommand<AngularServiceCommandParameters>
{
    private readonly IDependencyResolver resolver;

    public AngularServiceCommand(IDependencyResolver resolver)
    {
        this.resolver = resolver;
    }

    public override IGeneratorCommandResult Run()
    {
        Options options = this.resolver.Get<Options>();
        GeneratorOptions generatorOptions = options.Get<GeneratorOptions>();
        generatorOptions.SetFromParameter(this.Parameters);
        generatorOptions.Language = this.resolver.Get<AngularTypeScriptLanguage>();
        generatorOptions.SkipNamespace = true;
        generatorOptions.PropertiesToFields = true;
        TypeScriptOptions typeScriptOptions = options.Get<TypeScriptOptions>();
        typeScriptOptions.SetStrict(this.Parameters.RelativePath, this.resolver);

        AngularWriteConfiguration writeConfiguration = new();
        writeConfiguration.Service = new AngularWriteServiceConfiguration();
        writeConfiguration.Service.Name = this.Parameters.Name;
        writeConfiguration.Service.RelativePath = this.Parameters.RelativePath;
        writeConfiguration.Service.EndlessTries = this.Parameters.EndlessTries;
        writeConfiguration.Service.Timeouts = this.Parameters.Timeouts;
        writeConfiguration.Model.RelativePath = this.Parameters.RelativeModelPath;
        writeConfiguration.Service.HttpClient.Name = this.Parameters.HttpClient;
        writeConfiguration.Service.HttpClient.Import = this.Parameters.HttpClientUsing;
        writeConfiguration.Service.HttpClient.Get = this.Parameters.HttpClientGet;
        writeConfiguration.Service.HttpClient.HasGetOptions = this.Parameters.HttpClientGetOptions;
        writeConfiguration.Service.HttpClient.Post = this.Parameters.HttpClientPost;
        writeConfiguration.Service.HttpClient.HasPostOptions = this.Parameters.HttpClientPostOptions;
        writeConfiguration.Service.HttpClient.Patch = this.Parameters.HttpClientPatch;
        writeConfiguration.Service.HttpClient.HasPatchOptions = this.Parameters.HttpClientPatchOptions;
        writeConfiguration.Service.HttpClient.Put = this.Parameters.HttpClientPut;
        writeConfiguration.Service.HttpClient.HasPutOptions = this.Parameters.HttpClientPutOptions;
        writeConfiguration.Service.HttpClient.Delete = this.Parameters.HttpClientDelete;
        writeConfiguration.Service.HttpClient.HasDeleteOptions = this.Parameters.HttpClientDeleteOptions;

        this.resolver.Get<IOutput>().DeleteAllRelatedFiles(this.Parameters.RelativePath);

        this.resolver.Create<AngularServiceWriter>().Write(writeConfiguration);
        this.resolver.Create<TypeScriptIndexHelper>().Execute(this.Parameters.RelativePath);

        return this.Success();
    }
}
