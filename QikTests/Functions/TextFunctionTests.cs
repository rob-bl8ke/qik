using CygSoft.Qik;
using CygSoft.Qik.Functions;
using NUnit.Framework;

//
// TODO (Rob) Multi-line tests are just not there for indentLine() and doubleQuotes() see ../Files/MultieLine.* files.
namespace QikTests
{
    [TestFixture]
    class TextFunctionTests
    {
        [Test]
        public void BasicTextFunction_InputText_OutputsText()
        {
            var globalTable = new SymbolTable();

            var literalText = "Rob Blake";
            var expressionSymbol = new ExpressionSymbol("@authorName",
                new TextFunction("stub", literalText));
            
            Assert.AreEqual("@authorName", expressionSymbol.Symbol);
            Assert.AreEqual("Rob Blake", expressionSymbol.Value);
        }
    }
}
