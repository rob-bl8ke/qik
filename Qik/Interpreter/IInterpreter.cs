using System.Collections.Generic;
using CygSoft.Qik.Functions;
using Qik.UiWidgets;

namespace CygSoft.Qik
{
    public interface IInterpreter
    {
        ISymbolTerminal Interpret(IFunctionFactory functionFactory, string scriptText);
    }
}
