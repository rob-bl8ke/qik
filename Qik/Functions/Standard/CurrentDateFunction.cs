using System;
using System.Collections.Generic;
using System.Linq;

namespace CygSoft.Qik.Functions
{
    public class CurrentDateFunction : BaseFunction
    {
        public CurrentDateFunction(string name, List<IFunction> functionArguments)
            : base(name, functionArguments)
        {

        }

        public override string Execute()
        {
            if (functionArguments.Count() > 1)
                throw new Exception("Unexpected number of function arguments");

            string result = null;
            try
            {
                if (functionArguments.Count() == 1) 
                {
                    string dateFormatText = functionArguments[0].Execute();

                    if (dateFormatText != null && dateFormatText.Length >= 1)
                    {
                        string dateText = DateTime.Now.ToString(dateFormatText);
                        result = dateText;
                    }
                }
                else
                {
                    result = DateTime.Now.ToLongDateString();
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
