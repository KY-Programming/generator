using System;
using KY.Core.Dependency;
using KY.Generator.Command;
using KY.Generator.Core.Tests.Models;
using KY.Generator.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KY.Generator.Core.Tests
{
    [TestClass]
    public class CommandRegisterTests
    {
        private CommandRegister commands;

        [TestInitialize]
        public void Initialize()
        {
            this.commands = new CommandRegister(new DependencyResolver());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RegisterAmbiguousCommands()
        {
            this.commands
                .Register<TestCommand, TestConfiguration>("test", "group")
                .Register<TestCommand, TestConfiguration>("test", "group");
        }

        [TestMethod]
        [ExpectedException(typeof(AmbiguousCommandException))]
        public void TestAmbiguousCommandName()
        {
            this.commands
                .Register<TestCommand, TestConfiguration>("test", "group1")
                .Register<TestCommand, TestConfiguration>("test", "group2");
            this.commands.CreateConfiguration("test");
        }

        [TestMethod]
        [ExpectedException(typeof(CommandNotFoundException))]
        public void TestCommandNotFound()
        {
            this.commands.CreateConfiguration("test");
        }

        [TestMethod]
        public void TestOneGroupOneCommands()
        {
            this.commands.Register<TestCommand, TestConfiguration>("test1", "group1");
            Assert.IsNotNull(this.commands.CreateConfiguration("test1"));
        }

        [TestMethod]
        public void TestOneGroupMultipleCommands()
        {
            this.commands
                .Register<TestCommand, TestConfiguration>("test1", "group1")
                .Register<TestCommand, TestConfiguration>("test2", "group1");
            Assert.IsNotNull(this.commands.CreateConfiguration("test1"));
        }

        [TestMethod]
        public void TestMultipleGroupsOneCommands()
        {
            this.commands
                .Register<TestCommand, TestConfiguration>("test", "group1")
                .Register<TestCommand, TestConfiguration>("test", "group2");
            Assert.IsNotNull(this.commands.CreateConfiguration("group2-test"));
        }

        [TestMethod]
        public void TestMultipleGroupsMultipleCommands()
        {
            this.commands
                .Register<TestCommand, TestConfiguration>("test1", "group1")
                .Register<TestCommand, TestConfiguration>("test1", "group2")
                .Register<TestCommand, TestConfiguration>("test2", "group1")
                .Register<TestCommand, TestConfiguration>("test2", "group2");
            Assert.IsNotNull(this.commands.CreateConfiguration("group2-test1"));
        }
    }
}