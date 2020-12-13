using System.Collections.Generic;
using KY.Generator.Command;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KY.Generator.Core.Tests
{
    [TestClass]
    public class RawCommandReaderTest
    {
        [TestInitialize]
        public void Initialize()
        { }

        [TestMethod]
        public void TestEmptyParameters()
        {
            List<RawCommand> commands = RawCommandReader.Read();
            Assert.AreEqual(0, commands.Count);
        }

        [TestMethod]
        public void TestInvalidCommandOrder()
        {
            string[] parameters = { "-parameter", "command" };
            List<RawCommand> commands = RawCommandReader.Read(parameters);
            Assert.AreEqual(0, commands.Count);
        }

        [TestMethod]
        public void TestGlobalParameterFirst()
        {
            string[] parameters = { "--global", "command" };
            List<RawCommand> commands = RawCommandReader.Read(parameters);
            Assert.AreEqual(1, commands.Count);
            Assert.AreEqual("command", commands[0].Name);
            Assert.AreEqual(1, commands[0].Parameters.Count);
            Assert.AreEqual("global", commands[0].Parameters[0].Name);
        }

        [TestMethod]
        public void TestOneCommandWithOneSimpleParameter()
        {
            string[] parameters = { "command", "-parameter" };
            List<RawCommand> commands = RawCommandReader.Read(parameters);
            Assert.AreEqual(1, commands.Count);
            Assert.AreEqual("command", commands[0].Name);
            Assert.AreEqual(1, commands[0].Parameters.Count);
            Assert.AreEqual("parameter", commands[0].Parameters[0].Name);
        }

        [TestMethod]
        public void TestOneCommandWithTwoSimpleParameters()
        {
            string[] parameters = { "command", "-parameter1", "-parameter2" };
            List<RawCommand> commands = RawCommandReader.Read(parameters);
            Assert.AreEqual(1, commands.Count);
            Assert.AreEqual("command", commands[0].Name);
            Assert.AreEqual(2, commands[0].Parameters.Count);
            Assert.AreEqual("parameter1", commands[0].Parameters[0].Name);
            Assert.AreEqual("parameter2", commands[0].Parameters[1].Name);
        }

        [TestMethod]
        public void TestOneCommandWithOneValueParameter()
        {
            string[] parameters = { "command", "-parameter=test" };
            List<RawCommand> commands = RawCommandReader.Read(parameters);
            Assert.AreEqual(1, commands.Count);
            Assert.AreEqual("command", commands[0].Name);
            Assert.AreEqual(1, commands[0].Parameters.Count);
            Assert.AreEqual("parameter", commands[0].Parameters[0].Name);
            Assert.AreEqual("test", commands[0].Parameters[0].Value);
        }

        [TestMethod]
        public void TestOneCommandWithTwoValueParameters()
        {
            string[] parameters = { "command", "-parameter1=test1", "-parameter2=test2" };
            List<RawCommand> commands = RawCommandReader.Read(parameters);
            Assert.AreEqual(1, commands.Count);
            Assert.AreEqual("command", commands[0].Name);
            Assert.AreEqual(2, commands[0].Parameters.Count);
            Assert.AreEqual("parameter1", commands[0].Parameters[0].Name);
            Assert.AreEqual("test1", commands[0].Parameters[0].Value);
            Assert.AreEqual("parameter2", commands[0].Parameters[1].Name);
            Assert.AreEqual("test2", commands[0].Parameters[1].Value);
        }

        [TestMethod]
        public void TestTwoCommandsWithTheSameSimpleParameter()
        {
            string[] parameters = { "command1", "-parameter", "command2", "-parameter" };
            List<RawCommand> commands = RawCommandReader.Read(parameters);
            Assert.AreEqual(2, commands.Count);
            Assert.AreEqual("command1", commands[0].Name);
            Assert.AreEqual(1, commands[0].Parameters.Count);
            Assert.AreEqual("parameter", commands[1].Parameters[0].Name);
            Assert.AreEqual("command2", commands[1].Name);
            Assert.AreEqual(1, commands[1].Parameters.Count);
            Assert.AreEqual("parameter", commands[1].Parameters[0].Name);
        }

        [TestMethod]
        public void TestTwoCommandsWithDifferentSimpleParameter()
        {
            string[] parameters = { "command1", "-parameter1", "command2", "-parameter2" };
            List<RawCommand> commands = RawCommandReader.Read(parameters);
            Assert.AreEqual(2, commands.Count);
            Assert.AreEqual("command1", commands[0].Name);
            Assert.AreEqual(1, commands[0].Parameters.Count);
            Assert.AreEqual("parameter1", commands[0].Parameters[0].Name);
            Assert.AreEqual("command2", commands[1].Name);
            Assert.AreEqual(1, commands[1].Parameters.Count);
            Assert.AreEqual("parameter2", commands[1].Parameters[0].Name);
        }

        [TestMethod]
        public void TestTwoCommandsWithTheSameTwoSimpleParameters()
        {
            string[] parameters = { "command1", "-parameter1", "-parameter2", "command2", "-parameter1", "-parameter2" };
            List<RawCommand> commands = RawCommandReader.Read(parameters);
            Assert.AreEqual(2, commands.Count);
            Assert.AreEqual("command1", commands[0].Name);
            Assert.AreEqual(2, commands[0].Parameters.Count);
            Assert.AreEqual("parameter1", commands[0].Parameters[0].Name);
            Assert.AreEqual("parameter2", commands[0].Parameters[1].Name);
            Assert.AreEqual("command2", commands[1].Name);
            Assert.AreEqual(2, commands[1].Parameters.Count);
            Assert.AreEqual("parameter1", commands[1].Parameters[0].Name);
            Assert.AreEqual("parameter2", commands[1].Parameters[1].Name);
        }

        [TestMethod]
        public void TestTwoCommandsWithDifferentTwoSimpleParameters()
        {
            string[] parameters = { "command1", "-parameter1", "-parameter2", "command2", "-parameter3", "-parameter4" };
            List<RawCommand> commands = RawCommandReader.Read(parameters);
            Assert.AreEqual(2, commands.Count);
            Assert.AreEqual("command1", commands[0].Name);
            Assert.AreEqual(2, commands[0].Parameters.Count);
            Assert.AreEqual("parameter1", commands[0].Parameters[0].Name);
            Assert.AreEqual("parameter2", commands[0].Parameters[1].Name);
            Assert.AreEqual("command2", commands[1].Name);
            Assert.AreEqual(2, commands[1].Parameters.Count);
            Assert.AreEqual("parameter3", commands[1].Parameters[0].Name);
            Assert.AreEqual("parameter4", commands[1].Parameters[1].Name);
        }

        [TestMethod]
        public void TestTwoCommandsWithOneValueParameter()
        {
            string[] parameters = { "command1", "-parameter=test1", "command2", "-parameter=test2" };
            List<RawCommand> commands = RawCommandReader.Read(parameters);
            Assert.AreEqual(2, commands.Count);
            Assert.AreEqual("command1", commands[0].Name);
            Assert.AreEqual(1, commands[0].Parameters.Count);
            Assert.AreEqual("parameter", commands[0].Parameters[0].Name);
            Assert.AreEqual("test1", commands[0].Parameters[0].Value);
            Assert.AreEqual("command2", commands[1].Name);
            Assert.AreEqual(1, commands[1].Parameters.Count);
            Assert.AreEqual("parameter", commands[1].Parameters[0].Name);
            Assert.AreEqual("test2", commands[1].Parameters[0].Value);
        }

        [TestMethod]
        public void TestTwoCommandsWithTwoValueParameters()
        {
            string[] parameters = { "command1", "-parameter1=test1", "-parameter2=test2", "command2", "-parameter3=test3", "-parameter4=test4" };
            List<RawCommand> commands = RawCommandReader.Read(parameters);
            Assert.AreEqual(2, commands.Count);
            Assert.AreEqual("command1", commands[0].Name);
            Assert.AreEqual(2, commands[0].Parameters.Count);
            Assert.AreEqual("parameter1", commands[0].Parameters[0].Name);
            Assert.AreEqual("test1", commands[0].Parameters[0].Value);
            Assert.AreEqual("parameter2", commands[0].Parameters[1].Name);
            Assert.AreEqual("test2", commands[0].Parameters[1].Value);
            Assert.AreEqual("command2", commands[1].Name);
            Assert.AreEqual(2, commands[1].Parameters.Count);
            Assert.AreEqual("parameter3", commands[1].Parameters[0].Name);
            Assert.AreEqual("test3", commands[1].Parameters[0].Value);
            Assert.AreEqual("parameter4", commands[1].Parameters[1].Name);
            Assert.AreEqual("test4", commands[1].Parameters[1].Value);
        }

        [TestMethod]
        public void TestTwoCommandsWithOneSimpleAndOneGlobalSimpleParameter()
        {
            string[] parameters = { "command1", "-parameter1", "command2", "-parameter2", "--global" };
            List<RawCommand> commands = RawCommandReader.Read(parameters);
            Assert.AreEqual(2, commands.Count);
            Assert.AreEqual("command1", commands[0].Name);
            Assert.AreEqual(2, commands[0].Parameters.Count);
            Assert.AreEqual("parameter1", commands[0].Parameters[0].Name);
            Assert.AreEqual("global", commands[0].Parameters[1].Name);
            Assert.AreEqual("command2", commands[1].Name);
            Assert.AreEqual(2, commands[1].Parameters.Count);
            Assert.AreEqual("parameter2", commands[1].Parameters[0].Name);
            Assert.AreEqual("global", commands[1].Parameters[1].Name);
        }

        [TestMethod]
        public void TestTwoCommandsWithOneSimpleAndOneGlobalValueParameter()
        {
            string[] parameters = { "command1", "-parameter1", "command2", "-parameter2", "--global=everywhere" };
            List<RawCommand> commands = RawCommandReader.Read(parameters);
            Assert.AreEqual(2, commands.Count);
            Assert.AreEqual("command1", commands[0].Name);
            Assert.AreEqual(2, commands[0].Parameters.Count);
            Assert.AreEqual("parameter1", commands[0].Parameters[0].Name);
            Assert.AreEqual("global", commands[0].Parameters[1].Name);
            Assert.AreEqual("everywhere", commands[0].Parameters[1].Value);
            Assert.AreEqual("command2", commands[1].Name);
            Assert.AreEqual(2, commands[1].Parameters.Count);
            Assert.AreEqual("parameter2", commands[1].Parameters[0].Name);
            Assert.AreEqual("global", commands[1].Parameters[1].Name);
            Assert.AreEqual("everywhere", commands[1].Parameters[1].Value);
        }
    }
}