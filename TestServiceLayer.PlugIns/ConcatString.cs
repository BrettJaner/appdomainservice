namespace TestServiceLayer.PlugIns
{
    public class ConcatString : IStringOperation
    {
        public string OperationCode { get { return "CONCAT"; } }

        public string DoWork(string input)
        {
            return string.Concat(input, input);
        }
    }
}