using System;

namespace MyTestApplication
{
    //Request and Result objects must be marked as Serializable or inherit from MarshalByObjRef
    [Serializable]
    public class StringOperationResult
    {
        public string Result { get; private set; }

        public StringOperationResult(string result)
        {
            Result = result;
        }
    }
}