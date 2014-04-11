using System.AddIn.Contract;
using System.AddIn.Pipeline;

namespace Contracts
{
    [AddInContract]
    public interface IStringOperationService : IContract
    {
        string Execute(string opCode, string input);
    }
}