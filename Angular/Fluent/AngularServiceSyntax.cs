using KY.Generator.Angular.Commands;
using KY.Generator.Syntax;

namespace KY.Generator.Angular.Fluent;

internal class AngularServiceSyntax : IAngularServiceSyntax, IAngularHttpClientSyntax
{
    private readonly IExecutableSyntax syntax;
    private readonly AngularServiceCommand command;

    public AngularServiceSyntax(IExecutableSyntax syntax, AngularServiceCommand command)
    {
        this.syntax = syntax;
        this.command = command;
        this.command.Parameters.RelativeModelPath = this.syntax.Commands.OfType<AngularModelCommand>().FirstOrDefault()?.Parameters.RelativePath;
    }

    public IAngularServiceSyntax FormatNames(bool value = true)
    {
        this.command.Parameters.FormatNames = value;
        return this;
    }

    public IAngularServiceSyntax OutputPath(string path)
    {
        this.command.Parameters.RelativePath = path;
        return this;
    }

    public IAngularServiceSyntax Name(string name)
    {
        this.command.Parameters.Name = name;
        return this;
    }

    public IAngularHttpClientSyntax HttpClient(string type, string import)
    {
        this.command.Parameters.HttpClient = type;
        this.command.Parameters.HttpClientUsing = import;
        return this;
    }

    public IAngularHttpClientSyntax GetMethod(string name, Action<IAngularHttpClientMethodSyntax> optionsAction = null)
    {
        this.command.Parameters.HttpClientGet = name;
        optionsAction?.Invoke(new AngularHttpClientSyntax(this.command.Parameters.HttpClientGetOptions));
        return this;
    }

    public IAngularHttpClientSyntax PostMethod(string name, Action<IAngularHttpClientMethodSyntax> optionsAction = null)
    {
        this.command.Parameters.HttpClientPost = name;
        optionsAction?.Invoke(new AngularHttpClientSyntax(this.command.Parameters.HttpClientPostOptions));
        return this;
    }

    public IAngularHttpClientSyntax PutMethod(string name, Action<IAngularHttpClientMethodSyntax> optionsAction = null)
    {
        this.command.Parameters.HttpClientPut = name;
        optionsAction?.Invoke(new AngularHttpClientSyntax(this.command.Parameters.HttpClientPutOptions));
        return this;
    }

    public IAngularHttpClientSyntax PatchMethod(string name, Action<IAngularHttpClientMethodSyntax> optionsAction = null)
    {
        this.command.Parameters.HttpClientPatch = name;
        optionsAction?.Invoke(new AngularHttpClientSyntax(this.command.Parameters.HttpClientPatchOptions));
        return this;
    }

    public IAngularHttpClientSyntax DeleteMethod(string name, Action<IAngularHttpClientMethodSyntax> optionsAction = null)
    {
        this.command.Parameters.HttpClientDelete = name;
        optionsAction?.Invoke(new AngularHttpClientSyntax(this.command.Parameters.HttpClientDeleteOptions));
        return this;
    }
}
