using System;
using System.Collections.Generic;
using System.Linq;

namespace CygSoft.Qik.Functions
{
    //
    // TODO (Rob) This function needs to be rewritten so that the noOfTimes comes before the indentType (making the indentType optional).
    public class IndentFunction : BaseFunction
    {
        public IndentFunction(string name, List<IFunction> functionArguments)
            : base(name, functionArguments)
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
                string indentType = functionArguments[1].Execute();
                int noOfTimes = int.Parse(functionArguments[2].Execute());

                string indentedText = "";

                if (txt != null && txt.Length >= 1)
                {
                    if (indentType == "TAB")
                        indentedText = txt.PadLeft(txt.Length + noOfTimes, '\t');
                    else // SPACE
                        indentedText = txt.PadLeft(txt.Length + noOfTimes, ' ');

                    result = indentedText;
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
