using CygSoft.Qik;
using CygSoft.Qik.Functions;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace QikTests
{
    [TestFixture]
    class CurrentDateFunctionTests
    {

        [Test]
        public void Should_Return_Correct_Date_In_Default_Format_When_No_Argument_Specified()
        {
            var funcText = $"currentDate()";
            var output = TestHelpers.EvaluateFunction(funcText);
            Assert.AreEqual(DateTime.Now.ToLongDateString(), output);
        }

        [Test]
        public void Should_Return_Correct_Date_In_Default_Format_When_Format_Argument_Specified()
        {
            var funcText = $"currentDate(\"dd/MM/yyyy\")";
            var output = TestHelpers.EvaluateFunction(funcText);
            Assert.AreEqual(DateTime.Now.ToString("dd/MM/yyyy"), output);
        }

        [Test]
        public void CurrentDateFunction_RequestDate_ReturnsCurrentDate_1()
        {
            var globalTable = new SymbolTable();

            var functionArguments = new List<IFunction>
            {
                new TextFunction("stub", "dd/MM/yyyy")
            };

            var expressionSymbol = new ExpressionSymbol("@currentDate", new 
                CurrentDateFunction("stub", functionArguments));
            
            Assert.AreEqual("@currentDate", expressionSymbol.Symbol);
            Assert.AreEqual(DateTime.Now.ToString("dd/MM/yyyy"), expressionSymbol.Value);
        }
    }
}
