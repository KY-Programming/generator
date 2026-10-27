using KY.Core.Dependency;
using KY.Generator.Angular;
using KY.Generator.Angular.Configurations;
using KY.Generator.Angular.Writers;
using KY.Generator.Mappings;
using KY.Generator.Templates;
using KY.Generator.Transfer;
using KY.Generator.TypeScript;
using KY.Generator.TypeScript.Languages;
using KY.Generator.TypeScript.Writers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KY.Generator.Tests;

/// <summary>
/// Route and query values are concatenated into the url of the request. Without encoding a value like
/// <c>test#286</c> ends the url at the <c>#</c> - the browser takes the rest as fragment and the server only
/// receives <c>test</c>. The same goes for <c>&amp;</c>, <c>?</c>, <c>+</c>, <c>%</c>, <c>/</c> and spaces
/// </summary>
[TestClass]
public class AngularServiceWriterTests
{
    private DependencyResolver resolver = null!;
    private List<ITransferObject> transferObjects = null!;
    private List<FileTemplate> files = null!;

    [TestInitialize]
    public void Initialize()
    {
        this.transferObjects = [];
        this.files = [];
        this.resolver = new DependencyResolver();
        this.resolver.Bind<Options>().ToSingleton();
        this.resolver.Bind<ITypeMapping>().ToSingleton<TypeMapping>();
        this.resolver.Bind<List<ITransferObject>>().To(this.transferObjects);
        this.resolver.Bind<List<FileTemplate>>().To(this.files);
        this.resolver.Bind<IOptionsFactory>().ToSingleton<GeneratorOptionsFactory>();
        this.resolver.Bind<IOptionsFactory>().ToSingleton<TypeScriptOptionsFactory>();
        this.resolver.Bind<IOptionsFactory>().ToSingleton<AngularOptionsFactory>();
        Options.Register(() => this.resolver.Get<List<IOptionsFactory>>());
        Options.ClearGlobal();
        Options.GetGlobal<GeneratorOptions>().Language = new TypeScriptLanguage(this.resolver);

        HttpServiceTransferObject controller = new() { Name = "UserController", Route = "api/user" };
        HttpServiceActionTransferObject search = new() { Name = "Search", Route = "search", Type = HttpServiceActionTypeTransferObject.Get, ReturnType = new TypeTransferObject { Name = "string" } };
        search.Parameters.Add(new HttpServiceActionParameterTransferObject { Name = "query", FromQuery = true, Type = new TypeTransferObject { Name = "string" } });
        search.Parameters.Add(new HttpServiceActionParameterTransferObject { Name = "since", FromQuery = true, Type = new TypeTransferObject { Name = "Date" } });
        controller.Actions.Add(search);
        HttpServiceActionTransferObject get = new() { Name = "Get", Route = "{name}", Type = HttpServiceActionTypeTransferObject.Get, ReturnType = new TypeTransferObject { Name = "string" } };
        get.Parameters.Add(new HttpServiceActionParameterTransferObject { Name = "name", Inline = true, Type =new TypeTransferObject { Name = "string" } });
        controller.Actions.Add(get);
        this.transferObjects.Add(controller);
    }

    [TestCleanup]
    public void Cleanup()
    {
        Options.ClearGlobal();
    }

    [TestMethod]
    public void QueryValuesAreEncoded()
    {
        string output = this.Write();

        StringAssert.Contains(output, "url = this.append(url, query, \"query\");");
        StringAssert.Contains(output, "return url + (url.indexOf(\"?\") === -1 ? \"?\" : \"&\") + parameterName + \"=\" + encodeURIComponent(value.toString());");
    }

    [TestMethod]
    public void PathSegmentsAreEncoded()
    {
        string output = this.Write();

        StringAssert.Contains(output, "url = this.append(url, name, undefined, \"/\");");
        StringAssert.Contains(output, "return url + separator + (value === null || value === undefined ? \"\" : encodeURIComponent(value.toString()));");
    }

    [TestMethod]
    public void DatesAreEncodedThroughAppend()
    {
        string output = this.Write();

        StringAssert.Contains(output, "url = this.appendDate(url, since, \"since\");");
        StringAssert.Contains(output, "return this.append(url, date === null || date === undefined ? \"\" : typeof(date) === \"string\" ? date : date.toISOString(), parameterName, separator);");
    }

    [TestMethod]
    public void NoValueIsWrittenUnencoded()
    {
        string output = this.Write();

        Assert.AreEqual(2, output.Split("value.toString()").Length - 1, "Both branches of append write the value, both have to encode it");
        Assert.AreEqual(2, output.Split("encodeURIComponent(value.toString())").Length - 1);
    }

    private string Write()
    {
        AngularServiceWriter writer = this.resolver.Create<AngularServiceWriter>();
        writer.Write(new AngularWriteConfiguration { Service = new AngularWriteServiceConfiguration(), WriteModels = false });
        FileTemplate file = this.files.Single(x => x.Name.Contains("service"));
        Output.FileWriter cache = new(file.Options);
        new TypeScriptFileWriter().Write(file, cache);
        return cache.ToString();
    }
}
