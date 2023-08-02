
namespace CygSoft.Qik
{
    public interface ISymbolTable : ISymbolTerminal
    {
        void Clear();

        void AddSymbol(ISymbol symbol);      
    }

    //
    // TODO (Rob) This must come out into its own file.
    public interface ISymbolTerminal
    {
        string[] Symbols { get; }
        string[] InputSymbols { get; }

        void SetValue(string inputSymbol, string value);

        string GetValue(string symbol);   
    }

}