using ExpressionTrees.Task2.ExpressionMapping.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressionTrees.Task2.ExpressionMapping.Tests
{
    [TestClass]
    public class ExpressionMappingTests
    {
        // todo: add as many test methods as you wish, but they should be enough to cover basic scenarios of the mapping generator

        [TestMethod]
        public void TestMethod1()
        {
            var mapGenerator = new MappingGenerator();
            var mapper = mapGenerator.Generate<Foo, Bar>();

            var foo = new Foo()
            {
                Age = 20,
                FirstName = "FooName",
            };

            var res = mapper.Map(foo);

            Assert.IsNotNull(res);
            Assert.AreEqual(foo.FirstName, res.FirstName);
            Assert.AreEqual(foo.Age, res.Age);
        }
    }
}
