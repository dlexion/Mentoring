using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Composite.Task1.Tests
{
    [TestClass]
    public class LabelTextTests
    {
        public LabelTextTests()
        {
        }

        [TestMethod]
        public void Should_convert_to_string()
        {
            var label = new LabelText("myLabel");
            var labelStr = label.ConvertToString();

            labelStr.Should().Be("<label value='myLabel'/>");
        }
    }
}