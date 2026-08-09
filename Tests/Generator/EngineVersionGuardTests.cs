using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KY.Generator.Tests
{
    /// <summary>
    /// Two engine versions in one process only fail once a type of the one is looked up in the assembly of the other,
    /// with a message that names a type but no version. These tests pin the check that reports the mismatch where it
    /// happens - see <see cref="EngineVersionGuard" />.
    /// </summary>
    [TestClass]
    public class EngineVersionGuardTests
    {
        private const string RunningPath = @"C:\packages\ky.generator\10.0.0-preview.53\tools\netstandard2.0\KY.Generator.Common.Generator.dll";
        private const string CachePath = @"C:\packages\ky.generator.common\10.0.1-preview.11\lib\netstandard2.0\KY.Generator.Common.Fluent.dll";

        private EngineVersionGuard guard = null!;

        [TestInitialize]
        public void Initialize()
        {
            this.guard = new EngineVersionGuard("KY.Generator.Common.Generator", new Version(10, 0, 0, 0), RunningPath);
        }

        [TestMethod]
        public void MatchingCoreAssemblyIsAllowed()
        {
            Assert.IsNull(this.guard.Validate("KY.Generator.Common.Fluent", new Version(10, 0, 0, 0), new Version(10, 0, 0, 0), CachePath));
        }

        [TestMethod]
        public void CoreAssemblyOfAnotherEngineVersionIsRejected()
        {
            // The locators fall back to the newest version in the cache, so nobody asked for 10.0.1.0 here
            string? message = this.guard.Validate("KY.Generator.Common.Fluent", null, new Version(10, 0, 1, 0), CachePath);

            Assert.IsNotNull(message);
        }

        [TestMethod]
        public void MessageNamesBothVersionsAndBothPaths()
        {
            string? message = this.guard.Validate("KY.Generator.Common.Fluent", null, new Version(10, 0, 1, 0), CachePath);

            StringAssert.Contains(message, "10.0.0.0");
            StringAssert.Contains(message, "10.0.1.0");
            StringAssert.Contains(message, RunningPath);
            StringAssert.Contains(message, CachePath);
        }

        [TestMethod]
        public void ResolvedVersionOtherThanTheRequestedOneIsRejected()
        {
            string? message = this.guard.Validate("KY.Generator.TypeScript.Fluent", new Version(10, 0, 0, 0), new Version(10, 0, 1, 0), CachePath);

            Assert.IsNotNull(message);
        }

        /// <summary>
        /// Modules like KY.Generator.OData ship their own version and request their own generator with it
        /// (GenerateWith(UseSameVersion = true)), so they never have to match the running engine.
        /// </summary>
        [TestMethod]
        public void ModuleWithItsOwnVersionIsAllowed()
        {
            Assert.IsNull(this.guard.Validate("KY.Generator.OData.Generator", new Version(7, 6, 0, 0), new Version(7, 6, 0, 0), CachePath));
        }

        [TestMethod]
        public void ModuleWithoutRequestedVersionIsAllowed()
        {
            Assert.IsNull(this.guard.Validate("KY.Generator.OData.Generator", null, new Version(7, 6, 0, 0), CachePath));
        }

        [TestMethod]
        public void ForeignAssemblyIsAllowed()
        {
            Assert.IsNull(this.guard.Validate("Newtonsoft.Json", new Version(13, 0, 0, 0), new Version(13, 0, 3, 0), CachePath));
        }

        [TestMethod]
        public void AssemblyStartingWithTheEnginePrefixIsNotAnEngineAssembly()
        {
            Assert.IsFalse(EngineVersionGuard.IsEngineAssembly("KY.GeneratorSomething"));
            Assert.IsTrue(EngineVersionGuard.IsEngineAssembly("KY.Generator"));
            Assert.IsTrue(EngineVersionGuard.IsEngineAssembly("KY.Generator.Common"));
        }

        [TestMethod]
        public void CheckThrowsOnMismatch()
        {
            EngineVersionMismatchException exception = Assert.ThrowsException<EngineVersionMismatchException>(
                () => this.guard.Check("KY.Generator.Common.Fluent", null, new Version(10, 0, 1, 0), CachePath));

            StringAssert.Contains(exception.Message, CachePath);
        }

        [TestMethod]
        public void CheckPassesMatchingAssembly()
        {
            this.guard.Check("KY.Generator.Common.Fluent", null, new Version(10, 0, 0, 0), CachePath);
        }
    }
}
