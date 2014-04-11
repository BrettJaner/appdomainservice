using System.AddIn.Pipeline;

namespace AddInViews
{
    [AddInBase]
    public interface IStringOperationServiceAddInView
    {
        string Execute(string opCode, string input);
    }
}
