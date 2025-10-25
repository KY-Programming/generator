using KY.Generator.Syntax;

namespace KY.Generator.Angular;

internal class AngularServiceSyntax : IAngularServiceSyntax, IAngularHttpClientSyntax
{
    private readonly AngularServiceCommandParameters command;

    public AngularServiceSyntax(ExecutableSyntax syntax, AngularServiceCommandParameters command)
    {
        this.command = command;
        this.command.RelativeModelPath = syntax.Commands.OfType<AngularModelCommandParameters>().FirstOrDefault()?.RelativePath;
    }

    public IAngularServiceSyntax FormatNames(bool value = true)
    {
        this.command.FormatNames = value;
        return this;
    }

    public IAngularServiceSyntax OutputPath(string path)
    {
        this.command.RelativePath = path;
        return this;
    }

    public IAngularServiceSyntax Name(string name)
    {
        this.command.Name = name;
        return this;
    }

    public IAngularHttpClientSyntax HttpClient(string type, string import)
    {
        this.command.HttpClient = type;
        this.command.HttpClientUsing = import;
        return this;
    }

    public IAngularHttpClientSyntax GetMethod(string name, Action<IAngularHttpClientMethodSyntax>? optionsAction = null)
    {
        this.command.HttpClientGet = name;
        optionsAction?.Invoke(new AngularHttpClientSyntax(this.command.HttpClientGetOptions));
        return this;
    }

    public IAngularHttpClientSyntax PostMethod(string name, Action<IAngularHttpClientMethodSyntax>? optionsAction = null)
    {
        this.command.HttpClientPost = name;
        optionsAction?.Invoke(new AngularHttpClientSyntax(this.command.HttpClientPostOptions));
        return this;
    }

    public IAngularHttpClientSyntax PutMethod(string name, Action<IAngularHttpClientMethodSyntax>? optionsAction = null)
    {
        this.command.HttpClientPut = name;
        optionsAction?.Invoke(new AngularHttpClientSyntax(this.command.HttpClientPutOptions));
        return this;
    }

    public IAngularHttpClientSyntax PatchMethod(string name, Action<IAngularHttpClientMethodSyntax>? optionsAction = null)
    {
        this.command.HttpClientPatch = name;
        optionsAction?.Invoke(new AngularHttpClientSyntax(this.command.HttpClientPatchOptions));
        return this;
    }

    public IAngularHttpClientSyntax DeleteMethod(string name, Action<IAngularHttpClientMethodSyntax>? optionsAction = null)
    {
        this.command.HttpClientDelete = name;
        optionsAction?.Invoke(new AngularHttpClientSyntax(this.command.HttpClientDeleteOptions));
        return this;
    }
}
