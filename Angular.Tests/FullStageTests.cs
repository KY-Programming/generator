using System;
using KY.Core.Dependency;
using KY.Generator.Angular.Tests.Properties;
using KY.Generator.AspDotNet;
using KY.Generator.Csharp;
using KY.Generator.Mappings;
using KY.Generator.Output;
using KY.Generator.Reflection;
using KY.Generator.TypeScript;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KY.Generator.Angular.Tests
{
    [TestClass]
    public class FullStageTests
    {
        private IDependencyResolver resolver;
        private MemoryOutput output;

        [TestInitialize]
        public void Initialize()
        {
            this.resolver = new DependencyResolver();
            this.resolver.Bind<ITypeMapping>().ToSingleton<TypeMapping>();
            this.resolver.Create<CoreModule>().Initialize();
            this.resolver.Create<CsharpModule>().Initialize();
            this.resolver.Create<AspDotNetModule>().Initialize();
            this.resolver.Create<AngularModule>().Initialize();
            this.resolver.Create<ReflectionModule>().Initialize();
            this.resolver.Create<TypeScriptModule>().Initialize();
            this.output = this.resolver.Create<MemoryOutput>();
        }

        [TestMethod]
        public void CustomHttpClientType()
        {
            Assert.AreEqual(true, this.Run(Resources.custom_client_type), "Generation not successful");
            Assert.AreEqual(2, this.output.Files.Count);
            Assert.AreEqual(Resources.custom_values_service_ts, this.output.Files["src\\app\\services\\custom-values.service.ts"]);
        }

        private bool Run(string configuration)
        {
            // TODO: Implement test with new commands
            //List<ConfigurationSet> configurations = this.reader.Parse(configuration);
            //configurations.ForEach(x => x.Configurations.ForEach(y => y.AddHeader = false));
            //return this.runner.Run(configurations, this.output);
            throw new NotImplementedException();
        }
    }
}
