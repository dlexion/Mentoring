using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Composite.Task1.Tests
{
    [TestClass]
    public class InputTextTests
    {
        public InputTextTests()
        {
        }

        [TestMethod]
        public void Should_convert_to_string()
        {
            var input = new InputText("myInput", "myInputValue");
            var inputStr = input.ConvertToString();

            inputStr.Should().Be("<inputText name='myInput' value='myInputValue'/>");
        }
    }
}
