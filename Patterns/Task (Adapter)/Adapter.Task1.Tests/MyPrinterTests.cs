using Adapter.Task1;
using Adapter.Task1.Tests;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace TemplateMethod.Task1.Tests
{
    [TestClass]
    public class MyPrinterTests
    {
        private List<string> messages;
        private List<string> elements;

        public MyPrinterTests()
        {
            messages = new List<string>();

            elements = new List<string>()
            {
                "AA", "BB", "CC"
            };
        }

        [TestMethod]
        public void Should_print()
        {
            var mockWriter = new Mock<IWriter>();

            mockWriter.Setup(w => w.Write(It.IsAny<string>())).Callback<string>(s => messages.Add(s));

            var printer = new MyPrinterFactory().CreateMyPrinter(mockWriter.Object);

            printer.Print(new MyElements<string>(elements));

            elements.Should().BeEquivalentTo(messages);
        }
    }
}
