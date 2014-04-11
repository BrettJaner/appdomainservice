using System;
using System.Linq;

namespace MyTestApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var proxy = new StringOperationProxy();

            var result1 = proxy.ExecuteRequest(new StringOperationRequest("REVERSE", "PleaseReverseMe"));

            Console.WriteLine(string.Format("REVERSE Operation{0}", Environment.NewLine));
            Console.WriteLine(string.Format("Request = {0}", "PleaseReverseMe"));
            Console.WriteLine(string.Format("Result = {0}", result1.Result));
            Console.WriteLine();

            var result2 = proxy.ExecuteRequest(new StringOperationRequest("CONCAT", "PleaseConcatMe"));

            Console.WriteLine(string.Format("CONCAT Operation{0}", Environment.NewLine));
            Console.WriteLine(string.Format("Request = {0}", "PleaseConcatMe"));
            Console.WriteLine(string.Format("Result = {0}", result2.Result));
            Console.WriteLine();

            Console.WriteLine(string.Format("Assemblies loaded in main AppDomain{0}", Environment.NewLine));

            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
                Console.WriteLine(asm.ManifestModule.Name);
        }
    }
}
