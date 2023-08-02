using CygSoft.Qik;
using CygSoft.Qik.Functions;
using NUnit.Framework;
using System.Collections.Generic;

namespace QikTests
{
    [TestFixture]
    public class UpperCaseFunctionTests
    {
        [Test]
        public void UpperCaseFunction_InputLower_OutputsUpperCase_1()
        {
            var functionArguments = new List<IFunction>
            {
                new TextFunction("stub", "literaltext")
            };

            var expressionSymbol = new ExpressionSymbol("@classInstance",
                new UpperCaseFunction("stub", functionArguments));
            
            Assert.AreEqual("@classInstance", expressionSymbol.Symbol);
            Assert.AreEqual("LITERALTEXT", expressionSymbol.Value);
        }

        [Test]
        public void UpperCaseFunction_InputLower_OutputsUpperCase()
        {
            var funcText = $"upperCase(\"lowercase text\")";
            var output = TestHelpers.EvaluateFunction(funcText);
            Assert.AreEqual("LOWERCASE TEXT", output);
        }
    }
}
