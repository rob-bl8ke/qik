using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CygSoft.Qik.Functions
{
    public class ProperCaseFunction : BaseFunction
    {
        public ProperCaseFunction(string name, List<IFunction> functionArguments)
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
                    CultureInfo cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
                    result = cultureInfo.TextInfo.ToTitleCase(txt.ToLower());
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
