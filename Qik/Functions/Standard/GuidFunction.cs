using System;
using System.Collections.Generic;
using System.Linq;

namespace CygSoft.Qik.Functions
{
    public class GuidFunction : BaseFunction
    {
        public GuidFunction(string name, List<IFunction> functionArguments) : base(name, functionArguments) {}

        public override string Execute()
        {
            if (functionArguments.Count() > 1)
                throw new Exception("Unexpected number of function arguments");

            string result = null;
            try
            {
                if (functionArguments.Count() == 1) 
                {
                    string txt = functionArguments[0].Execute();
                    result = txt == "u" ? Guid.NewGuid().ToString().ToUpper() : Guid.NewGuid().ToString();
                }
                else
                {
                    result = Guid.NewGuid().ToString();
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
