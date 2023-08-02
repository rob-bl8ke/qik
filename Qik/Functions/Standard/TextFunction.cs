
namespace CygSoft.Qik.Functions
{
    public class TextFunction : BaseFunction
    {
        private readonly string text;

        public TextFunction(string name, string text) : base(name)
        {
            this.text = text;
        }

        public override string Execute()
        {
            return this.text;
        }
    }
}
