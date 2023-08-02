using System.Collections.Generic;
using Antlr4.Runtime;
using CygSoft.Qik.Antlr;
using CygSoft.Qik.Functions;
using Qik.UiWidgets;

namespace CygSoft.Qik
{
    public class Interpreter : IInterpreter
    {
        public ISymbolTerminal Interpret(IFunctionFactory functionFactory, string scriptText)
        {
            var symbolTable = new SymbolTable();
            symbolTable.Clear();

            InterpretInputs(symbolTable, scriptText);
            InterpretExpressions(symbolTable, functionFactory, scriptText);

            return symbolTable;
        }

        private void InterpretExpressions(ISymbolTable symbolTable, IFunctionFactory functionFactory, string scriptText)
        {
            var inputStream = new AntlrInputStream(scriptText);
            var lexer = new QikTemplateLexer(inputStream);
            var tokens = new CommonTokenStream(lexer);
            var parser = new QikTemplateParser(tokens);

            var tree = parser.template();

            var expressionVisitor = new ExpressionVisitor(symbolTable, functionFactory);
            expressionVisitor.Visit(tree);
        }

        private void InterpretInputs(ISymbolTable symbolTable, string scriptText)
        {
            var inputStream = new AntlrInputStream(scriptText);
            var lexer = new QikTemplateLexer(inputStream);
            var tokens = new CommonTokenStream(lexer);
            var parser = new QikTemplateParser(tokens);

            var tree = parser.template();

            var controlVisitor = new UserInputVisitor(symbolTable);
            controlVisitor.Visit(tree);
        }
    }
}
