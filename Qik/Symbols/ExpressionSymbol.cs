using System;
using CygSoft.Qik.Functions;

namespace CygSoft.Qik
{
    public class ExpressionSymbol : BaseSymbol
    {
        private readonly IFunction func;

        public ExpressionSymbol(string symbol, IFunction func) : base(symbol)
            => this.func = func ?? throw new ArgumentNullException($"{nameof(func)} cannot be null.");

        public override string Value => func.Execute();
    }
}
