namespace MyTestApplication
{
    public interface IStringOperation
    {
        string OperationCode { get; }
        string DoWork(string input);
    }
}