namespace CygSoft.Qik.Functions
{
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class QikFunctionAttribute : System.Attribute
    {
        public readonly string Symbol;

        public QikFunctionAttribute(string symbol)
        {
            this.Symbol = symbol;
        }
    }
}
