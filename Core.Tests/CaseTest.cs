using KY.Generator.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KY.Generator.Core.Tests
{
    [TestClass]
    public class CaseTest
    {
        /****************
         *  PascalCase  *
         ****************/

        [TestMethod]
        public void TestPascalAllLower()
        {
            Assert.AreEqual("Alllower", "alllower".ToPascalCase());
        }
        [TestMethod]
        public void TestPascalAllLowerNumber()
        {
            Assert.AreEqual("Alllower1", "alllower1".ToPascalCase());
        }
        [TestMethod]
        public void TestPascalAllLowerNumbers()
        {
            Assert.AreEqual("Alllower123", "alllower123".ToPascalCase());
        }

        [TestMethod]
        public void TestPascalAllUpper()
        {
            Assert.AreEqual("Allupper", "ALLUPPER".ToPascalCase());
        }

        [TestMethod]
        public void TestPascalAllUpperNumber()
        {
            Assert.AreEqual("Allupper1", "ALLUPPER1".ToPascalCase());
        }

        [TestMethod]
        public void TestPascalAllUpperNumbers()
        {
            Assert.AreEqual("Allupper123", "ALLUPPER123".ToPascalCase());
        }

        [TestMethod]
        public void TestPascalCamelCase()
        {
            Assert.AreEqual("CamelCase", "camelCase".ToPascalCase());
        }

        [TestMethod]
        public void TestPascalSpecialCamelCase()
        {
            Assert.AreEqual("CamelACase", "camelACase".ToPascalCase());
        }

        [TestMethod]
        public void TestPascalSpecialNumberCamelCase()
        {
            Assert.AreEqual("CamelA1Case", "camelA1Case".ToPascalCase());
        }

        [TestMethod]
        public void TestPascalSpecialCamelCaseNumber()
        {
            Assert.AreEqual("CamelACase1", "camelACase1".ToPascalCase());
        }

        [TestMethod]
        public void TestPascalSpecialCamelCaseNumbers()
        {
            Assert.AreEqual("CamelACase123", "camelACase123".ToPascalCase());
        }

        [TestMethod]
        public void TestPascalPascalCase()
        {
            Assert.AreEqual("TitleCase", "Title Case".ToPascalCase());
        }

        [TestMethod]
        public void TestPascalTitleCaseNumber()
        {
            Assert.AreEqual("TitleCase1", "Title Case 1".ToPascalCase());
        }

        [TestMethod]
        public void TestPascalTitleCaseNumbers()
        {
            Assert.AreEqual("TitleCase123", "Title Case 1 23".ToPascalCase());
        }

        [TestMethod]
        public void TestPascalSnakeCase()
        {
            Assert.AreEqual("SnakeCase", "snake_case".ToPascalCase());
        }

        [TestMethod]
        public void TestPascalSnakeCaseNumber()
        {
            Assert.AreEqual("SnakeCase1", "snake_case1".ToPascalCase());
        }

        [TestMethod]
        public void TestPascalSnakeCaseNumbers()
        {
            Assert.AreEqual("SnakeCase123", "snake_case_1_23".ToPascalCase());
        }

        [TestMethod]
        public void TestPascalUpperSnakeCase()
        {
            Assert.AreEqual("UpperSnakeCase", "UPPER_SNAKE_CASE".ToPascalCase());
        }

        [TestMethod]
        public void TestPascalDarwinCase()
        {
            Assert.AreEqual("DarwinCase", "Darwin_Case".ToPascalCase());
        }

        [TestMethod]
        public void TestPascalKebabCase()
        {
            Assert.AreEqual("KebabCase", "kebab-case".ToPascalCase());
        }

        [TestMethod]
        public void TestPascalKebabCaseNumber()
        {
            Assert.AreEqual("KebabCase1", "kebab-case-1".ToPascalCase());
        }

        [TestMethod]
        public void TestPascalKebabCaseNumbers()
        {
            Assert.AreEqual("KebabCase123", "kebab-case1-23".ToPascalCase());
        }

        /****************
         *  CamelCase  *
         ****************/

        [TestMethod]
        public void TestCamelAllLower()
        {
            Assert.AreEqual("alllower", "alllower".ToCamelCase());
        }
        [TestMethod]
        public void TestCamelAllLowerNumber()
        {
            Assert.AreEqual("alllower1", "alllower1".ToCamelCase());
        }
        [TestMethod]
        public void TestCamelAllLowerNumbers()
        {
            Assert.AreEqual("alllower123", "alllower123".ToCamelCase());
        }

        [TestMethod]
        public void TestCamelAllUpper()
        {
            Assert.AreEqual("allupper", "ALLUPPER".ToCamelCase());
        }

        [TestMethod]
        public void TestCamelAllUpperNumber()
        {
            Assert.AreEqual("allupper1", "ALLUPPER1".ToCamelCase());
        }

        [TestMethod]
        public void TestCamelAllUpperNumbers()
        {
            Assert.AreEqual("allupper123", "ALLUPPER123".ToCamelCase());
        }

        [TestMethod]
        public void TestCamelCamelCase()
        {
            Assert.AreEqual("camelCase", "camelCase".ToCamelCase());
        }

        [TestMethod]
        public void TestCamelSpecialCamelCase()
        {
            Assert.AreEqual("camelACase", "camelACase".ToCamelCase());
        }

        [TestMethod]
        public void TestCamelSpecialNumberCamelCase()
        {
            Assert.AreEqual("camelA1Case", "camelA1Case".ToCamelCase());
        }

        [TestMethod]
        public void TestCamelSpecialCamelCaseNumber()
        {
            Assert.AreEqual("camelACase1", "camelACase1".ToCamelCase());
        }

        [TestMethod]
        public void TestCamelSpecialCamelCaseNumbers()
        {
            Assert.AreEqual("camelACase123", "camelACase123".ToCamelCase());
        }

        [TestMethod]
        public void TestCamelTitleCase()
        {
            Assert.AreEqual("titleCase", "Title Case".ToCamelCase());
        }

        [TestMethod]
        public void TestCamelCamelCaseNumber()
        {
            Assert.AreEqual("titleCase1", "Title Case 1".ToCamelCase());
        }

        [TestMethod]
        public void TestCamelCamelCaseNumbers()
        {
            Assert.AreEqual("titleCase123", "Title Case 1 23".ToCamelCase());
        }

        [TestMethod]
        public void TestCamelSnakeCase()
        {
            Assert.AreEqual("snakeCase", "snake_case".ToCamelCase());
        }

        [TestMethod]
        public void TestCamelSnakeCaseNumber()
        {
            Assert.AreEqual("snakeCase1", "snake_case1".ToCamelCase());
        }

        [TestMethod]
        public void TestCamelSnakeCaseNumbers()
        {
            Assert.AreEqual("snakeCase123", "snake_case_1_23".ToCamelCase());
        }

        [TestMethod]
        public void TestCamelUpperSnakeCase()
        {
            Assert.AreEqual("upperSnakeCase", "UPPER_SNAKE_CASE".ToCamelCase());
        }

        [TestMethod]
        public void TestCamelDarwinCase()
        {
            Assert.AreEqual("darwinCase", "Darwin_Case".ToCamelCase());
        }

        [TestMethod]
        public void TestCamelKebabCase()
        {
            Assert.AreEqual("kebabCase", "kebab-case".ToCamelCase());
        }

        [TestMethod]
        public void TestCamelKebabCaseNumber()
        {
            Assert.AreEqual("kebabCase1", "kebab-case-1".ToCamelCase());
        }

        [TestMethod]
        public void TestCamelKebabCaseNumbers()
        {
            Assert.AreEqual("kebabCase123", "kebab-case1-23".ToCamelCase());
        }
    }
}