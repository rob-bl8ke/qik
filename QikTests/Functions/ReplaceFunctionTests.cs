using CygSoft.Qik;
using CygSoft.Qik.Functions;
using NUnit.Framework;
using System.Collections.Generic;

namespace QikTests
{
    [TestFixture]
    class ReplaceFunctionTests
    {
        [Test]
        public void ReplaceFunction_InputText_ReplacesCorrectly_1()
        {
            var globalTable = new SymbolTable();

            var functionArguments = new List<IFunction>();
            functionArguments.Add(new TextFunction("stub", "Dashboard Usage"));
            functionArguments.Add(new TextFunction("stub", " "));
            functionArguments.Add(new TextFunction("stub", "_"));

            var expressionSymbol = new ExpressionSymbol("@classInstance",
                new ReplaceFunction("stub", functionArguments));
            
            Assert.AreEqual("@classInstance", expressionSymbol.Symbol);
            Assert.AreEqual("Dashboard_Usage", expressionSymbol.Value);
        }

        [Test]
        public void ReplaceFunction_InputText_ReplacesCorrectly()
        {
            var funcText = $"replace(\"literal text ya all\", \" \", \"_\")";
            var output = TestHelpers.EvaluateFunction(funcText);
            Assert.AreEqual("literal_text_ya_all", output);
        }
    }
}
