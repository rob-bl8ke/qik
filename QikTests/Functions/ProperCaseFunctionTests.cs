using CygSoft.Qik;
using CygSoft.Qik.Functions;
using NUnit.Framework;
using System.Collections.Generic;

namespace QikTests
{
    [TestFixture]
    public class ProperCaseFunctionTests
    {
        [Test]
        public void ProperCaseFunction_InputUpperCase_OutputsProperCase_1()
        {
            var globalTable = new SymbolTable();

            var functionArguments = new List<IFunction>
            {
                new TextFunction("stub", "LITERAL TEXT")
            };

            var expressionSymbol = new ExpressionSymbol("@toProperCase",
                new ProperCaseFunction("stub", functionArguments));
                
            Assert.AreEqual("@toProperCase", expressionSymbol.Symbol);
            Assert.AreEqual("Literal Text", expressionSymbol.Value);
        }

        [Test]
        public void ProperCaseFunction_InputUpperCase_OutputsProperCase()
        {
            var funcText = $"properCase(\"PROPERCASE TEXT\")";
            var output = TestHelpers.EvaluateFunction(funcText);
            Assert.AreEqual("Propercase Text", output);
        }
    }
}
