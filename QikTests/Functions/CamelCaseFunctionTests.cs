using CygSoft.Qik;
using CygSoft.Qik.Functions;
using NUnit.Framework;
using System.Collections.Generic;

namespace QikTests
{
    [TestFixture]
    public class CamelCaseFunctionTests
    {
        [Test]
        public void CamelCaseFunction_InputPascalCase_OutputsCamelCase_1()
        {
            var globalTable = new SymbolTable();

            var functionArguments = new List<IFunction>
            {
                new TextFunction("stub", "LiteralText")
            };

            var expressionSymbol = new ExpressionSymbol("@classInstance",  
                new CamelCaseFunction("stub", functionArguments));
            
            Assert.AreEqual("@classInstance", expressionSymbol.Symbol);
            Assert.AreEqual("literalText", expressionSymbol.Value);
        }

        [Test]
        public void CamelCaseFunction_InputPascalCase_OutputsCamelCase()
        {
            var funcText = $"camelCase(\"LiteralText\")";
            var output = TestHelpers.EvaluateFunction(funcText);
            Assert.AreEqual("literalText", output);
        }
    }
}
