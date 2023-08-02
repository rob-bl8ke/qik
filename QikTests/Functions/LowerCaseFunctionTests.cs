using CygSoft.Qik;
using CygSoft.Qik.Functions;
using NUnit.Framework;
using System.Collections.Generic;

namespace QikTests
{
    [TestFixture]
    public class LowerCaseFunctionTests
    {
        [Test]
        public void LowerCaseFunction_InputCAPS_OutputsLowerCase_1()
        {
            var globalTable = new SymbolTable();

            var functionArguments = new List<IFunction>
            {
                new TextFunction("stub", "LITERALTEXT")
            };

            var expressionSymbol = new ExpressionSymbol("@classInstance",
                new LowerCaseFunction("stub", functionArguments));
            
            Assert.AreEqual("@classInstance", expressionSymbol.Symbol);
            Assert.AreEqual("literaltext", expressionSymbol.Value);
        }

        [Test]
        public void LowerCaseFunction_InputCAPS_OutputsLowerCase()
        {
            var funcText = $"lowerCase(\"LOWERCASE TEXT\")";
            var output = TestHelpers.EvaluateFunction(funcText);
            Assert.AreEqual("lowercase text", output);
        }
    }
}
