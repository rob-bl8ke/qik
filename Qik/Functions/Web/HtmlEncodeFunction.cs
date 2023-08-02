using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CygSoft.Qik.Functions
{
    public class HtmlEncodeFunction : BaseFunction
    {
        public HtmlEncodeFunction(string name, List<IFunction> functionArguments) : base(name, functionArguments)
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
                    result = HttpUtility.HtmlEncode(txt); 
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
