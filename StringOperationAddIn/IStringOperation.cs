namespace StringOperationAddIn
{
    internal interface IStringOperation
    {
        string OperationCode { get; }

        string DoWork(string input);
    }
}
