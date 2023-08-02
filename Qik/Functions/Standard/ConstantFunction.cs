
namespace CygSoft.Qik.Functions
{
    public class ConstantFunction : BaseFunction
    {
        private readonly string text;

        public ConstantFunction(string name, string text)
            : base(name)
        {
            this.text = text;
        }

        public override string Execute() => text;
    }
}
