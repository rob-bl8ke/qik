using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CygSoft.Qik
{
    public class SymbolTable : ISymbolTable
    {
        private readonly Dictionary<string, ISymbol> table = new Dictionary<string, ISymbol>();

        public string[] Symbols
        {
            get
            {
                if (table.Keys.Select(r => r).Any())
                    return table.Keys.Select(r => r).ToArray();
                else
                    return new string[0];
            }
        }

        public string[] InputSymbols
        {
            get
            {
                if (table.Values.OfType<InputSymbol>().Any())
                    return table.Values.OfType<InputSymbol>().Select(r => r.Symbol).ToArray();
                else
                    return new string[0];
            }
        }

        public void Clear()
        {
            table.Clear();
        }

        public void SetValue(string inputSymbol, string value)
        {
            if (table.ContainsKey(inputSymbol) && table[inputSymbol] is InputSymbol)
            {
                (table[inputSymbol] as InputSymbol).SetValue(value);
            }
            else
            {
                throw new KeyNotFoundException($"Input symbol does not exist for {inputSymbol}");
            }
        }

        public void AddSymbol(ISymbol symbol)
        {
            if (!table.ContainsKey(symbol.Symbol))
                table.Add(symbol.Symbol, symbol);
        }

        public string GetValue(string symbol)
        {
            if (table.ContainsKey(symbol))
            {
                ISymbol baseSymbol = table[symbol];
                return baseSymbol.Value;
            }
            return null;
        }
    }
}
