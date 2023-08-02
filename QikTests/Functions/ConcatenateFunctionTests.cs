using NUnit.Framework;

namespace QikTests
{
    [TestFixture]
    class ConcatenateFunctionTests
    {
        [Test]
        public void ConcatenateFunction_Input3Strings_ConcatenatesToSingleString()
        {
            var concatExpression = "\"hello\" + \" \" + \"world\"";
            var output = TestHelpers.EvaluateFunction(concatExpression);
            Assert.AreEqual("hello world", output);
        }
    }
}
