using System.ServiceModel;

namespace TestServiceLayer
{
    [ServiceContract]
    public interface IStringOperationPortal
    {
        [OperationContract]
        string Execute(string operationCode, string input);
    }
}