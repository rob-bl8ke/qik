using CygSoft.Qik;
using CygSoft.Qik.Functions;
using NUnit.Framework;
using System.Collections.Generic;

namespace QikTests
{
    [TestFixture]
    class RemoveSpacesFunctionTests
    {
        [Test]
        public void RemoveSpacesFunction_InputSpacedText_RemovesSpaces_1()
        {
            var globalTable = new SymbolTable();

            var functionArguments = new List<IFunction>
            {
                new TextFunction("stub", "literal text")
            };

            var expressionSymbol = new ExpressionSymbol("@classInstance",
                new RemoveSpacesFunction("stub", functionArguments));
            
            Assert.AreEqual("@classInstance", expressionSymbol.Symbol);
            Assert.AreEqual("literaltext", expressionSymbol.Value);
        }

        [Test]
        public void RemoveSpacesFunction_InputSpacedText_RemovesSpaces()
        {
            var funcText = $"removeSpaces(\"literal text ya all\")";
            var output = TestHelpers.EvaluateFunction(funcText);
            Assert.AreEqual("literaltextyaall", output);
        }
    }
}
