using System.Linq;

namespace TestServiceLayer.PlugIns
{
    public class ReverseString : IStringOperation
    {
        public string OperationCode { get { return "REVERSE"; } }

        public string DoWork(string input)
        {
            return string.Concat(input.Reverse());
        }
    }
}