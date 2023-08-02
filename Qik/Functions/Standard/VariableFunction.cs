using System;

namespace CygSoft.Qik.Functions
{
    public class VariableFunction : BaseFunction
    {
        private readonly string symbol;
        protected ISymbolTable symbolTable = null;

        public VariableFunction(string name, ISymbolTable symbolTable, string symbol)
            : base(name)
        {
            this.symbol = symbol;
            this.symbolTable = symbolTable ?? throw new ArgumentNullException($"{nameof(symbolTable)} cannot be null.");
        }

        public override string Execute()
        {
            return symbolTable.GetValue(this.symbol);
        }
    }
}
