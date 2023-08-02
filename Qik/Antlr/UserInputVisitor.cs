using Antlr4.Runtime.Misc;

namespace CygSoft.Qik.Antlr
{
    internal class UserInputVisitor : QikTemplateBaseVisitor<string>
    {
        private readonly ISymbolTable symbolTable;

        internal UserInputVisitor(ISymbolTable symbolTable)
        {
            this.symbolTable = symbolTable;
        }

        public override string VisitInputDecl([NotNull] QikTemplateParser.InputDeclContext context)
        {
            var inputSymbol = new InputSymbol(context.VARIABLE().GetText());
            inputSymbol.SetValue(context.STRING().GetText().StripOuterQuotes());

            symbolTable.AddSymbol(inputSymbol);
            
            return base.VisitInputDecl(context);
        }
    }
}
