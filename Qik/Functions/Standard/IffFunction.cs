using System;

namespace CygSoft.Qik.Functions
{
    public class IifFunction : IFunction
    {
        private readonly string comparisonOperator = "";
        private readonly IFunction leftOperand;
        private readonly IFunction rightOperand;
        private readonly IFunction trueExpression;
        private readonly IFunction falseExpression;

        public string Name { get; }

        public IifFunction(string name, IFunction leftOperand, IFunction rightOperand, string comparisonOperator, 
            IFunction trueExpression, IFunction falseExpression)
        {
            this.leftOperand = leftOperand;
            this.rightOperand = rightOperand;
            this.trueExpression = trueExpression;
            this.falseExpression = falseExpression;
            this.comparisonOperator = comparisonOperator;
        }

        public string Execute()
        {
            try
            {
                if (comparisonOperator == "==")
                {
                    if (leftOperand.Execute() == rightOperand.Execute())
                    {
                        return trueExpression.Execute();
                    }
                    else
                    {
                        return falseExpression.Execute();
                    }
                }
                else if (comparisonOperator == "!=")
                {
                    if (leftOperand.Execute() != rightOperand.Execute())
                    {
                        return trueExpression.Execute();
                    }
                    else
                    {
                        return falseExpression.Execute();
                    }
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
}
