using System;
using System.Collections.Generic;
using System.Linq;

namespace CygSoft.Qik.Functions
{
    public class Base64EncodeFunction : BaseFunction
    {
        public Base64EncodeFunction(string name, List<IFunction> functionArguments) : base(name, functionArguments)
        {

        }

        public override string Execute()
        {
            if (functionArguments.Count() != 1)
                throw new Exception("Unexpected number of function arguments");

            string result = null;
            try
            {
                string plainText = functionArguments[0].Execute();

                if (plainText != null && plainText.Length >= 1)
                {
                    var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
                    result = System.Convert.ToBase64String(plainTextBytes);
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
