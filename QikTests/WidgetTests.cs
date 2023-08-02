using CygSoft.Qik;
using CygSoft.Qik.Functions;
using NUnit.Framework;
using Qik.UiWidgets;

namespace QikTests
{
    [TestFixture]
    class WidgetTests
    {

        [Test]
        public void Should_Be_Expected_Value_For_Simple_Text_Comparison_Case_Check()
        {
            UiWidgetFactory widgetFactory = new UiWidgetFactory();
            UiWidget[] widgets = widgetFactory.BuildFromScript(
                @"
                    [title = ""Choose a Color"", type = ""text"" ] @testVal => ""Green"";
                "
            );

            Assert.That(widgets.Length, Is.EqualTo(1));
            Assert.That(widgets[0].Title, Is.EqualTo("Choose a Color"));
            Assert.That(widgets[0].Type, Is.EqualTo("text"));
        }
    }
}
