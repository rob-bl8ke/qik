using NUnit.Framework;

namespace QikTests
{
    [TestFixture]
    class UrlDecodeFunctionTests
    {
        [Test]
        public void Should_Decode_Correctly()
        {
            var text = @"rob.xchangedocs%2bthomas.bishop.bishops%40gmail.com";
            var funcText = $"urlDecode(\"{text}\")";
            var output = TestHelpers.EvaluateFunction(funcText);
            Assert.AreEqual(@"rob.xchangedocs+thomas.bishop.bishops@gmail.com", output);
        }

        [Test]
        public void Should_Decode_Correctly_Again()
        {
            var text = @"rob.xchangedocs%2Bthomas.bishop.bishops%40gmail.com";
            var funcText = $"urlDecode(\"{text}\")";
            var output = TestHelpers.EvaluateFunction(funcText);
            Assert.AreEqual(@"rob.xchangedocs+thomas.bishop.bishops@gmail.com", output);
        }
    }
}