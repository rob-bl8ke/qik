using System;
using System.Collections.Generic;
using System.Linq;

namespace CygSoft.Qik.Functions
{
    public class CamelCaseFunction : BaseFunction
    {
        public CamelCaseFunction(string name, List<IFunction> functionArguments) : base(name, functionArguments)
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
                    string firstChar = txt.Substring(0, 1);
                    string theRest = txt.Substring(1, txt.Length - 1);
                    result = firstChar.ToLower() + theRest;
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
