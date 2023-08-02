using System;
using System.Collections.Generic;

namespace CygSoft.Qik.Functions
{
    public class IfCase
    {
        private readonly string comparisonOperator = "";
        private readonly IFunction leftOperand;
        private readonly IFunction rightOperand;

        public IFunction  ResultFunction { get; init; }

        public IfCase(IFunction leftOperand, IFunction rightOperand, string comparisonOperator, IFunction resultFunc)
        {
            this.leftOperand = leftOperand;
            this.rightOperand = rightOperand;
            this.comparisonOperator = comparisonOperator;
            this.ResultFunction = resultFunc;
        }

        public bool Matches()
        {
            try
            {
                if (comparisonOperator == "==")
                {
                    return leftOperand.Execute() == rightOperand.Execute();
                }
                else if (comparisonOperator == "!=")
                {
                    return leftOperand.Execute() != rightOperand.Execute();
                }
                else
                {
                    throw new Exception("Unidentified operator");
                }
            }
            catch (Exception exception)
            {
                throw new Exception("Unspecified function construction error.", exception);
            }
        }
    }

    public class IfFunction : IFunction
    {
        public string Name { get; init; }

        private readonly IfCase ifFunction;
        private readonly List<IfCase> elseIfFunctions;
        private readonly IFunction elseFunction;

        public IfFunction(string name, IfCase ifFunction, List<IfCase> elseIfFunctions, IFunction elseFunction)
        {
            this.Name = name;
            this.ifFunction = ifFunction;
            this.elseIfFunctions = elseIfFunctions;
            this.elseFunction = elseFunction;
        }

        public string Execute()
        {
            try
            {
                var result = elseFunction.Execute();

                if (ifFunction.Matches()) 
                { 
                    result = ifFunction.ResultFunction.Execute();
                }
                else
                {
                    foreach (var elseIfFunction in elseIfFunctions)
                    {
                        if (elseIfFunction.Matches()) 
                        {
                            result = elseIfFunction.ResultFunction.Execute();
                            break;
                        }
                    }
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
