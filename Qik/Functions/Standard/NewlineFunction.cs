using System;

namespace CygSoft.Qik.Functions
{
    public class NewlineFunction : BaseFunction
    {
        public NewlineFunction(string name)
            : base(name)
        {
        }

        public override string Execute()
        {
            return Environment.NewLine;
        }
    }
}
