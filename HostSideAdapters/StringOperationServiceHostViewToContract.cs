using Contracts;
using HostViews;
using System.AddIn.Pipeline;

namespace HostSideAdapters
{
    [HostAdapter]
    public class StringOperationServiceHostViewToContract : IStringOperationServiceHostView
    {
        private IStringOperationService _contract;

        private ContractHandle _lifetime;

        public StringOperationServiceHostViewToContract(IStringOperationService contract)
        {
            _contract = contract;

            _lifetime = new ContractHandle(contract);
        }

        public string Execute(string opCode, string input)
        {
            return _contract.Execute(opCode, input);
        }
    }
}
