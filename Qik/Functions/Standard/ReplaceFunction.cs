using System;
using System.Collections.Generic;
using System.Linq;

namespace CygSoft.Qik.Functions
{
    public class ReplaceFunction : BaseFunction
    {
        public ReplaceFunction(string name, List<IFunction> functionArguments)
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
                List<string> textResults = new List<string>();
                foreach (BaseFunction funcArg in functionArguments)
                {
                    textResults.Add(funcArg.Execute());
                }

                string targetText = functionArguments[0].Execute();
                string textToReplace = functionArguments[1].Execute();
                string replacementText = functionArguments[2].Execute();

                if (targetText != null && targetText.Length >= 1)
                {
                    result = targetText.Replace(textToReplace, replacementText);
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
