using System;

namespace MyTestApplication
{
    //Request and Result objects must be marked as Serializable or inherit from MarshalByObjRef
    [Serializable]
    public class StringOperationRequest
    {
        public string OperationCode { get; private set; }
        public string Input { get; private set; }

        public StringOperationRequest(string operationCode, string input)
        {
            OperationCode = operationCode;
            Input = input;
        }
    }
}