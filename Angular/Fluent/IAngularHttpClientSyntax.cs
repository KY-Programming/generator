namespace KY.Generator.Angular;

public interface IAngularHttpClientSyntax : IAngularServiceSyntax
{
    IAngularHttpClientSyntax GetMethod(string name, Action<IAngularHttpClientMethodSyntax> optionsAction = null);
    IAngularHttpClientSyntax PostMethod(string name, Action<IAngularHttpClientMethodSyntax> optionsAction = null);
    IAngularHttpClientSyntax PutMethod(string name, Action<IAngularHttpClientMethodSyntax> optionsAction = null);
    IAngularHttpClientSyntax PatchMethod(string name, Action<IAngularHttpClientMethodSyntax> optionsAction = null);
    IAngularHttpClientSyntax DeleteMethod(string name, Action<IAngularHttpClientMethodSyntax> optionsAction = null);
}
