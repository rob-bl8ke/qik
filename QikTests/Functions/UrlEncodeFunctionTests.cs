using NUnit.Framework;

namespace QikTests
{
    [TestFixture]
    class UrlEncodeFunctionTests
    {
        [Test]
        public void Should_Encode_Correctly()
        {
            var text = @"rob.xchangedocs+thomas.bishop.bishops@gmail.com";
            var funcText = $"urlEncode(\"{text}\")";
            var output = TestHelpers.EvaluateFunction(funcText);
            Assert.AreEqual(@"rob.xchangedocs%2bthomas.bishop.bishops%40gmail.com", output);
        }
    }
}