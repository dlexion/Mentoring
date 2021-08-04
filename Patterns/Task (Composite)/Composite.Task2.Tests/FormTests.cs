using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Composite.Task2.Tests
{
    [TestClass]
    public class FormTests
    {
        public FormTests()
        {
        }

        [TestMethod]
        public void Should_convert_form_with_two_components_to_string()
        {
            var label = new LabelText("myLabel");
            var input = new InputText("myInput", "myInputValue");

            var form = new Form("myForm");
            form.AddComponent(label);
            form.AddComponent(input);

            var formStr = form.ConvertToString();
            var expected = @"<form name='myForm'>
 <label value='myLabel'/>
 <inputText name='myInput' value='myInputValue'/>
</form>";

            formStr.Should().Be(expected);
        }

        [TestMethod]
        public void Should_convert_form_with_internal_form_with_two_components_to_string()
        {
            var labelInternal = new LabelText("myLabelInternal");
            var inputInternal = new InputText("myInputInternal", "myInputInternalValue");

            var formInternal = new Form("myFormInternal");
            formInternal.AddComponent(labelInternal);
            formInternal.AddComponent(inputInternal);

            var label = new LabelText("myLabel");
            var input = new InputText("myInput", "myInputValue");

            var form = new Form("myForm");
            form.AddComponent(label);
            form.AddComponent(input);
            form.AddComponent(formInternal);

            var formStr = form.ConvertToString();
            var expected = @"<form name='myForm'>
 <label value='myLabel'/>
 <inputText name='myInput' value='myInputValue'/>
 <form name='myFormInternal'>
  <label value='myLabelInternal'/>
  <inputText name='myInputInternal' value='myInputInternalValue'/>
 </form>
</form>";

            formStr.Should().Be(expected);
        }
    }
}
