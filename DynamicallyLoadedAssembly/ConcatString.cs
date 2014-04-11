using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyTestApplication;

namespace DynamicallyLoadedAssembly
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
