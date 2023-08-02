
namespace CygSoft.Qik.Functions
{
    public class IntegerFunction : BaseFunction
    {
        private readonly string text;

        public IntegerFunction(string name, string text)
            : base(name)
        {
            this.text = text;
        }

        public override string Execute()
        {
            return this.text;
        }
    }
}
