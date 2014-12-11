using System.ServiceModel;

namespace MyTestApplication
{
    [ServiceContract]
    public interface IStringOperationPortal
    {
        [OperationContract]
        string Execute(string operationCode, string input);
    }
}