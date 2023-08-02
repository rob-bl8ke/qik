using System.Collections.Generic;

namespace CygSoft.Qik.Functions
{
    public abstract class BaseFunction : IFunction
    {
        protected List<IFunction> functionArguments;
        public string Name { get; }

        public BaseFunction(string name, List<IFunction> functionArguments = null)
        {
            Name = name;
            this.functionArguments = functionArguments ?? new List<IFunction>();
        }

        public abstract string Execute();
    }
}
