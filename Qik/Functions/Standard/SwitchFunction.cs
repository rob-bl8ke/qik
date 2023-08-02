using System;
using System.Collections.Generic;

namespace CygSoft.Qik.Functions
{
    public class SwitchFunction : IFunction
    {
        public string Name { get; init; }

        private readonly IFunction subjectFunction;
        private readonly Dictionary<string, IFunction> caseFunctions;
        private readonly IFunction elseFunction;

        public SwitchFunction(string name, IFunction subjectFunction, Dictionary<string, IFunction> caseFunctions, IFunction elseFunction)
        {
            this.Name = name;
            this.subjectFunction = subjectFunction;
            this.caseFunctions = caseFunctions;
            this.elseFunction = elseFunction;
        }

        public string Execute()
        {
            try
            {
                var result = elseFunction.Execute();
                var compareValue = subjectFunction.Execute();

                foreach (var caseFunction in caseFunctions)
                {
                    if (caseFunction.Key == compareValue)
                        result = caseFunction.Value.Execute();
                }

                return result;
            }
            catch (Exception exception)
            {
                throw new Exception("Unspecified function construction error.", exception);
            }
        }
    }
}
