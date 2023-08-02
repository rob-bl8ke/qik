namespace Qik.UiWidgets
{
    public class UiWidget
    {
        public readonly string Symbol;
        public string Title { get; internal set; }
        public string Type { get; internal set; }

        internal UiWidget(string symbol) => Symbol = symbol;
    }
}