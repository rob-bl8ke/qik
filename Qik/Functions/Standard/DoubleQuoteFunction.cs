using System;
using System.Collections.Generic;
using System.Linq;

namespace CygSoft.Qik.Functions
{
    public class DoubleQuoteFunction : BaseFunction
    {
        public DoubleQuoteFunction(string name, List<IFunction> functionArguments)
            : base(name, functionArguments)
        {

        }

        public override string Execute()
        {
            if (functionArguments.Count() != 1)
                throw new Exception("Unexpected number of function arguments");

            string result = null;
            try
            {
                string txt = functionArguments[0].Execute();

                if (txt != null && txt.Length >= 1)
                {
                    return "\"" + txt + "\"";
                }
                result = txt;
            }
            catch (Exception exception)
            {
                throw new Exception("Unspecified function construction error.", exception);
            }
            return result;
        }
    }
}
