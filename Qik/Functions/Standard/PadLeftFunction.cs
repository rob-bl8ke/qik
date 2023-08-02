using System;
using System.Collections.Generic;
using System.Linq;

namespace CygSoft.Qik.Functions
{
    public class PadLeftFunction : BaseFunction
    {
        public PadLeftFunction(string name, List<IFunction> functionArguments) : base(name, functionArguments)
        {

        }

        public override string Execute()
        {
            if (functionArguments.Count() != 3)
                throw new Exception("Unexpected number of function arguments");

            string result = null;
            try
            {
                string txt = functionArguments[0].Execute();
                char character = functionArguments[1].Execute()[0];
                int amount = int.Parse(functionArguments[2].Execute());

                if (txt != null && txt.Length >= 1)
                {
                    result = txt.PadLeft(amount, character);
                }
            }
            catch (Exception exception)
            {
                throw new Exception("Unspecified function construction error.", exception);
            }
            return result;
        }
    }
}
