using System.Collections.Generic;
using KY.Core.Dependency;
using KY.Generator.Command;
using KY.Generator.Configuration;
using KY.Generator.Configurations;
using KY.Generator.Core.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KY.Generator.Core.Tests
{
    [TestClass]
    public class CommandReaderTests
    {
        private CommandRegister commands;
        private CommandReader reader;

        [TestInitialize]
        public void Initialize()
        {
            this.commands = new CommandRegister(new DependencyResolver());
            this.reader = new CommandReader(this.commands);
        }

        [TestMethod]
        public void TestReadString()
        {
            this.commands.Register<TestCommand, StringConfiguration>("test");
            List<string> parameters = new List<string>{ "test", "-test=Test"};
            StringConfiguration configuration = this.reader.Read(parameters) as StringConfiguration;
            Assert.IsNotNull(configuration, "Read failed");
            Assert.AreEqual("Test", configuration.Test);
        }

        [TestMethod]
        public void TestReadInt()
        {
            this.commands.Register<TestCommand, IntConfiguration>("test");
            List<string> parameters = new List<string>{ "test", "-test=123"};
            IntConfiguration configuration = this.reader.Read(parameters) as IntConfiguration;
            Assert.IsNotNull(configuration, "Read failed");
            Assert.AreEqual(123, configuration.Test);
        }

        [TestMethod]
        public void TestReadBoolTrue()
        {
            this.commands.Register<TestCommand, BoolConfiguration>("test");
            List<string> parameters = new List<string>{ "test", "-test=true"};
            BoolConfiguration configuration = this.reader.Read(parameters) as BoolConfiguration;
            Assert.IsNotNull(configuration, "Read failed");
            Assert.AreEqual(true, configuration.Test);
        }

        [TestMethod]
        public void TestReadBoolFalse()
        {
            this.commands.Register<TestCommand, BoolConfiguration>("test");
            List<string> parameters = new List<string>{ "test", "-test=false"};
            BoolConfiguration configuration = this.reader.Read(parameters) as BoolConfiguration;
            Assert.IsNotNull(configuration, "Read failed");
            Assert.AreEqual(false, configuration.Test);
        }

        [TestMethod]
        public void TestReadBoolEmpty()
        {
            this.commands.Register<TestCommand, BoolConfiguration>("test");
            List<string> parameters = new List<string>{ "test", "-test"};
            BoolConfiguration configuration = this.reader.Read(parameters) as BoolConfiguration;
            Assert.IsNotNull(configuration, "Read failed");
            Assert.AreEqual(true, configuration.Test);
        }

        [TestMethod]
        public void TestReadCustomType()
        {
            this.commands.Register<TestCommand, CustomTypeConfiguration>("test");
            List<string> parameters = new List<string>{ "test", "-test=~1~2~3~"};
            CustomTypeConfiguration configuration = this.reader.Read(parameters) as CustomTypeConfiguration;
            Assert.IsNotNull(configuration, "Read failed");
            Assert.IsNotNull(configuration.Test);
            Assert.AreEqual(123, configuration.Test.Value);
        }

        private class StringConfiguration : TestConfiguration
        {
            public string Test { get; set; }
        }

        private class IntConfiguration : TestConfiguration
        {
            public int Test { get; set; }
        }

        private class BoolConfiguration : TestConfiguration
        {
            public bool Test { get; set; }
        }

        private class CustomTypeConfiguration : TestConfiguration
        {
            [ConfigurationPropertyConverter(typeof(CustomTypeConverter))]
            public CustomType Test { get; set; }

            public class CustomType
            {
                public int Value { get; set; }
            }

            public class CustomTypeConverter : ConfigurationPropertyConverter
            {
                public override object Convert(string value)
                {
                    return new CustomType { Value = int.Parse(value.Replace("~", string.Empty)) };
                }
            }
        }
    }
}