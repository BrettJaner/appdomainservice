using System.Linq;
using MyTestApplication;

namespace DynamicallyLoadedAssembly
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
