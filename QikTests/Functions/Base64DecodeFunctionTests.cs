using NUnit.Framework;

namespace QikTests
{
    [TestFixture]
    class Base64DecodeFunctionTests
    {
        [Test]
        public void Should_Base64_Decode_Correctly()
        {
            var text = "UmVkIFNveA==";
            var funcText = $"base64Decode(\"{text}\")";
            var output = TestHelpers.EvaluateFunction(funcText);
            Assert.AreEqual("Red Sox", output);
        }
    }
}