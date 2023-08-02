
namespace CygSoft.Qik.Functions
{
    public interface IFunction
    {
        string Name { get; }

        string Execute();
    }
}