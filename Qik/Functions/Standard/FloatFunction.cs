
namespace CygSoft.Qik.Functions
{
    public class FloatFunction : BaseFunction
    {
        private readonly string text;

        public FloatFunction(string name, string text)
            : base(name)
        {
            this.text = text;
        }

        public override string Execute()
        {
            return text;
        }
    }
}
