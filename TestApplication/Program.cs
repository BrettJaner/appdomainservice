using System;
using System.Linq;

namespace TestApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var proxy = new StringAppProxy();
            using (var channelFactory = proxy.CreateChannelFactory())
            {
                var portal = channelFactory.CreateChannel();

                string result = portal.Execute("REVERSE", "PleaseReverseMe");

                Console.WriteLine(string.Format("REVERSE Operation{0}", Environment.NewLine));
                Console.WriteLine(string.Format("Request = {0}", "PleaseReverseMe"));
                Console.WriteLine(string.Format("Result = {0}", result));
                Console.WriteLine();

                result = portal.Execute("CONCAT", "PleaseConcatMe");

                Console.WriteLine(string.Format("CONCAT Operation{0}", Environment.NewLine));
                Console.WriteLine(string.Format("Request = {0}", "PleaseConcatMe"));
                Console.WriteLine(string.Format("Result = {0}", result));
                Console.WriteLine();
            }

            Console.WriteLine(string.Format("Assemblies loaded in main AppDomain{0}", Environment.NewLine));

            foreach (var asmName in AppDomain.CurrentDomain.GetAssemblies().Select(x => x.ManifestModule.Name).OrderBy(x => x))
                Console.WriteLine(asmName);

            proxy.Reset();
        }
    }
}