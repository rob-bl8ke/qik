namespace CygSoft.Qik
{
    public abstract class BaseSymbol : ISymbol 
    {
        public string Symbol { get; }

        public abstract string Value { get; }

        public BaseSymbol(string symbol) => Symbol = symbol;
    }
}
