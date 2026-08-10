using System.Collections.Generic;
using System.Linq;
using KY.Generator.AspDotNet;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KY.Generator.Tests
{
    /// <summary>
    /// An action without [MapToApiVersion] is generated with the lowest version its controller declares, so the order
    /// of the [ApiVersion] attributes must not change the generated url - see <see cref="ApiVersionComparer" />.
    /// </summary>
    [TestClass]
    public class ApiVersionComparerTests
    {
        [TestMethod]
        public void MinorVersionIsComparedAsNumber()
        {
            AssertLower("1.0", "1.1");
            AssertLower("1.9", "1.10");
        }

        [TestMethod]
        public void MajorVersionIsComparedAsNumber()
        {
            AssertLower("2.0", "10.0");
        }

        [TestMethod]
        public void MissingMinorVersionCountsAsZero()
        {
            AssertEqual("1", "1.0");
            AssertLower("1", "1.1");
        }

        [TestMethod]
        public void VersionWithStatusIsLowerThanTheReleasedOne()
        {
            AssertLower("1.0-beta", "1.0");
            AssertLower("1.0-alpha", "1.0-beta");
            AssertEqual("1.0-Beta", "1.0-beta");
        }

        [TestMethod]
        public void GroupVersionWins()
        {
            AssertLower("2013-08-06.2.0", "2013-08-07.1.0");
        }

        [TestMethod]
        public void UnparsableVersionIsComparedAsText()
        {
            AssertLower("alpha", "beta");
        }

        [TestMethod]
        public void LowestVersionIsIndependentOfTheDeclaredOrder()
        {
            List<string> versions = ["2.0", "1.0", "1.0-beta"];
            Assert.AreEqual("1.0-beta", versions.OrderBy(x => x, ApiVersionComparer.Instance).First());
            versions.Reverse();
            Assert.AreEqual("1.0-beta", versions.OrderBy(x => x, ApiVersionComparer.Instance).First());
        }

        private static void AssertLower(string lower, string higher)
        {
            Assert.IsTrue(ApiVersionComparer.Instance.Compare(lower, higher) < 0, $"{lower} has to be lower than {higher}");
            Assert.IsTrue(ApiVersionComparer.Instance.Compare(higher, lower) > 0, $"{higher} has to be higher than {lower}");
        }

        private static void AssertEqual(string left, string right)
        {
            Assert.AreEqual(0, ApiVersionComparer.Instance.Compare(left, right), $"{left} has to be equal to {right}");
        }
    }
}
