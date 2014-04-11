using System;
using System.AddIn.Hosting;
using System.Collections.ObjectModel;
using HostViews;

namespace Host
{
    class Program
    {
        static void Main()
        {
            // Update the cache files of the pipeline segments and add-ins. 
            string[] warnings = AddInStore.Update(Environment.CurrentDirectory);
            foreach (string warning in warnings)
            {
                Console.WriteLine(warning);
            }

            // Search for add-ins of type IStringOperationHostView (the host view of the add-in).
            Collection<AddInToken> tokens = AddInStore.FindAddIns(typeof(IStringOperationServiceHostView), Environment.CurrentDirectory);

            // Ask the user which add-in they would like to use.
            AddInToken calcToken = ChooseStringOperation(tokens);

            // Activate the selected AddInToken in a new application domain  
            // with the Internet trust level.
            IStringOperationServiceHostView op = calcToken.Activate<IStringOperationServiceHostView>(AddInSecurityLevel.Internet);

            // Run the add-in.
            RunStringOperation(op);

            var controller = AddInController.GetAddInController(op);

            controller.Shutdown();

            Console.ReadLine();
        }

        private static AddInToken ChooseStringOperation(Collection<AddInToken> tokens)
        {
            if (tokens.Count == 0)
            {
                Console.WriteLine("No String Operations are available");
                return null;
            }
            Console.WriteLine("Available String Operations: ");
            // Show the token properties for each token in the AddInToken collection  
            // (tokens), preceded by the add-in number in [] brackets. 
            int tokNumber = 1;
            foreach (AddInToken tok in tokens)
            {
                Console.WriteLine("\t[{0}]: {1} - {2}\n\t{3}\n\t\t {4}\n\t\t {5} - {6}",
                    tokNumber,
                    tok.Name,
                    tok.AddInFullName,
                    tok.AssemblyName,
                    tok.Description,
                    tok.Version,
                    tok.Publisher);
                tokNumber++;
            }
            Console.WriteLine("Which string operation do you want to use?");
            String line = Console.ReadLine();
            int selection;
            if (Int32.TryParse(line, out selection))
            {
                if (selection <= tokens.Count)
                {
                    return tokens[selection - 1];
                }
            }
            Console.WriteLine("Invalid selection: {0}. Please choose again.", line);
            return ChooseStringOperation(tokens);
        }

        private static void RunStringOperation(IStringOperationServiceHostView op)
        {
            string result1 = op.Execute("REVERSE", "PleaseReverseMe");

            Console.WriteLine("REVERSE Operation{0}", Environment.NewLine);
            Console.WriteLine("Request = {0}", "PleaseReverseMe");
            Console.WriteLine("Result = {0}", result1);
            Console.WriteLine();

            var result2 = op.Execute("CONCAT", "PleaseConcatMe");

            Console.WriteLine("CONCAT Operation{0}", Environment.NewLine);
            Console.WriteLine("Request = {0}", "PleaseConcatMe");
            Console.WriteLine("Result = {0}", result2);
            Console.WriteLine();

            Console.WriteLine("Assemblies loaded in main AppDomain{0}", Environment.NewLine);

            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
                Console.WriteLine(asm.ManifestModule.Name);
        }
    }
}
