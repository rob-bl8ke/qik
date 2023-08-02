using CygSoft.Qik;
using CygSoft.Qik.Functions;
using NUnit.Framework;
using System.Collections.Generic;

namespace QikTests
{
    [TestFixture]
    public class DoubleQuotedFunctionTests
    {
        [Test]
        public void DoubleQuoteFunction_InputText_OutputsDoubleQuotedText()
        {
            var globalTable = new SymbolTable();

            var functionArguments = new List<IFunction>
            {
                new TextFunction("stub", "literal text")
            };

            var expressionSymbol = new ExpressionSymbol("@classInstance",  
                new DoubleQuoteFunction("stub", functionArguments));

            Assert.AreEqual("@classInstance", expressionSymbol.Symbol);
            Assert.AreEqual("\"literal text\"", expressionSymbol.Value);
        }

        [Test]
        public void DoubleQuoteFunction_New_InputText_OutputsDoubleQuotedText()
        {
            var funcText = $"doubleQuote(\"quote me\")";
            var output = TestHelpers.EvaluateFunction(funcText);
            Assert.AreEqual("\"quote me\"", output);
        }
    }
}