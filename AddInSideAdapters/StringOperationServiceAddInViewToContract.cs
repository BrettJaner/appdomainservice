using System.AddIn.Pipeline;
using AddInViews;
using Contracts;

namespace AddInSideAdapters
{
    [AddInAdapter]
    public class StringOperationServiceAddInViewToContract : ContractBase, IStringOperationService
    {
        private IStringOperationServiceAddInView _view;

        public StringOperationServiceAddInViewToContract(IStringOperationServiceAddInView view)
        {
            _view = view;
        }   

        public string Execute(string opCode, string input)
        {
            return _view.Execute(opCode, input);
        }
    }
}
