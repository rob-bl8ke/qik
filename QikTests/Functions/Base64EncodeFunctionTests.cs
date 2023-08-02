using NUnit.Framework;

namespace QikTests
{
    [TestFixture]
    class Base64EncodeFunctionTests
    {
        [Test]
        public void Should_Base64_Encode_Correctly()
        {
            var text = "Red Sox";
            var funcText = $"base64Encode(\"{text}\")";
            var output = TestHelpers.EvaluateFunction(funcText);
            Assert.AreEqual("UmVkIFNveA==", output);
        }
    }
}