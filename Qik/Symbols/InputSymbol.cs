namespace CygSoft.Qik
{
    public class InputSymbol : BaseSymbol
    {
        private string value = null;
        public override string Value => value;

        public InputSymbol(string symbol) : base(symbol) {}

        public void SetValue(string value) => this.value = value;
    }
}
