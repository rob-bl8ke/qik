using NUnit.Framework;

namespace QikTests
{
    [TestFixture]
    class HtmlDecodeFunctionTests
    {
        [Test]
        public void Should_Decode_Correctly()
        {
            var text = @"Hello &#39;World&#39;";
            var funcText = $"htmlDecode(\"{text}\")";
            var output = TestHelpers.EvaluateFunction(funcText);
            Assert.AreEqual(@"Hello 'World'", output);
        }
    }
}