namespace TestServiceLayer
{
    public interface IStringOperation
    {
        string OperationCode { get; }
        string DoWork(string input);
    }
}