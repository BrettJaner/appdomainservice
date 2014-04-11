using System;
using System.AddIn;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AddInViews;

namespace StringOperationAddIn
{
    [AddIn("String Operation AddIn", Version = "1.0.0.0")]
    public class StringOperationService : IStringOperationServiceAddInView
    {
        private static readonly Dictionary<string, IStringOperation> Cache = new Dictionary<string, IStringOperation>();

        static StringOperationService()
        {
            Cache = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => !t.IsAbstract)
                .Where(t => typeof(IStringOperation).IsAssignableFrom(t))
                .Select(x => (IStringOperation)Assembly.GetExecutingAssembly().CreateInstance(x.FullName))
                .ToDictionary(x => x.OperationCode);
        }

        public string Execute(string opCode, string input)
        {
            if (!Cache.ContainsKey(opCode))
                throw new InvalidOperationException(string.Format("StringOperation Type does not exist for OperationCode={0}", opCode));

            var operation = Cache[opCode];

            return operation.DoWork(input);
        }
    }
}