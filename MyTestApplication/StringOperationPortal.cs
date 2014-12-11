using System;
using System.Collections.Generic;
using System.Linq;

namespace MyTestApplication
{
    public class StringOperationPortal : IStringOperationPortal
    {
        private static readonly Dictionary<string, IStringOperation> _cache = new Dictionary<string, IStringOperation>();

        static StringOperationPortal()
        {
            _cache = AppDomain.CurrentDomain.GetAssemblies()
                            .SelectMany(asm => asm.GetExportedTypes())
                            .Where(t => !t.IsAbstract)
                            .Where(t => typeof(IStringOperation).IsAssignableFrom(t))
                            .Select(t => (IStringOperation)t.Assembly.CreateInstance(t.FullName))
                            .ToDictionary(x => x.OperationCode);
        }

        public string Execute(string operationCode, string input)
        {
            if (!_cache.ContainsKey(operationCode))
                throw new InvalidOperationException(string.Format("StringOperation Type does not exist for OperationCode={0}", operationCode));

            var operation = _cache[operationCode];

            return operation.DoWork(input);
        }
    }
}