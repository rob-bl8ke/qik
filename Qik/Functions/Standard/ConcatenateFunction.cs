using System;
using System.Collections.Generic;

namespace CygSoft.Qik.Functions
{
    public class ConcatenateFunction : BaseFunction
    {
        private readonly List<IFunction> functions = new List<IFunction>();

        public ConcatenateFunction(string name)
            : base(name)
        {
        }

        public override string Execute()
        {
            string result = null;
            try
            {
                foreach (BaseFunction func in functions)
                {
                    result += func.Execute();
                }
            }
            catch (Exception exception)
            {
                throw new Exception("Unspecified function construction error.", exception);
            }
            return result;
        }
        public void AddFunction(IFunction func)
        {
            functions.Add(func);
        }

    }
}
