using CygSoft.Qik;
using NUnit.Framework;

namespace QikTests
{
    [TestFixture]
    class StringExtensionTests
    {
        [Test]
        public void Should_Strip_Outer_Quotes()
        {
            string value = "\"Hello World\"";
            Assert.AreEqual("Hello World", value.StripOuterQuotes());
        }
    }
}