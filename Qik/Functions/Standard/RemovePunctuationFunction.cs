using System;
using System.Collections.Generic;
using System.Linq;

namespace CygSoft.Qik.Functions
{
    public class RemovePunctuationFunction : BaseFunction
    {
        public RemovePunctuationFunction(string name, List<IFunction> functionArguments)
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
                    result = txt.Where(c => !char.IsPunctuation(c)).Aggregate("", (current, c) => current + c);
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


// http://stackoverflow.com/questions/421616/how-can-i-strip-punctuation-from-a-string
//string s = "sxrdct?fvzguh,bij.";
//var sb = new StringBuilder();

//foreach (char c in s)
//{
//   if (!char.IsPunctuation(c))
//      sb.Append(c);
//}

//s = sb.ToString();