using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CygSoft.Qik.Functions
{
    public class AbbreviateFunction : BaseFunction
    {
        public AbbreviateFunction(string name, List<IFunction> functionArguments)
            : base(name, functionArguments) {}

        public override string Execute()
        {            
            if (functionArguments.Count() != 1)
                throw new Exception("Unexpected number of function arguments");

            string result = null;

            try
            {
                string txt = functionArguments[0].Execute();

                if (!string.IsNullOrWhiteSpace(txt))
                {
                    var builder = new StringBuilder();

                    foreach (var chr in txt.ToCharArray())
                    {
                        if (char.IsUpper(chr))
                        {
                            builder.Append(' ');
                            builder.Append(chr);
                    
                        }
                        else if (chr == '_')
                        {
                            builder.Append(' ');
                        }
                        else
                        {
                            builder.Append(chr);
                        }
                    }
                    
                    var results = builder.ToString()
                        .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                        .Select(str => str[0]).ToArray();
                    
                    result = new string(results);
                    return result;
                }
                else
                    return null;
            }
            catch (Exception exception)
            {
                throw new Exception("Unspecified function construction error.", exception);
            }
        }
    }
}
