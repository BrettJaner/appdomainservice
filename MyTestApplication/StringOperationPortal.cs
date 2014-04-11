using System;
using System.Collections.Generic;
using System.Linq;
using AppDomainService;

namespace MyTestApplication
{
    public class StringOperationPortal : AppDomainPortal<StringOperationRequest, StringOperationResult>
    {
        private Dictionary<string, IStringOperation> _cache = new Dictionary<string, IStringOperation>();

        protected override void OnAssemblyLoaded()
        {
            _cache = Assembly.GetTypes()
                            .Where(t => !t.IsAbstract)
                            .Where(t => typeof(IStringOperation).IsAssignableFrom(t))
                            .Select(x => (IStringOperation)Assembly.CreateInstance(x.FullName))
                            .ToDictionary(x => x.OperationCode);
        }

        protected override StringOperationResult Execute(StringOperationRequest request)
        {
            if (!_cache.ContainsKey(request.OperationCode))
                throw new InvalidOperationException(string.Format("StringOperation Type does not exist for OperationCode={0}", request.OperationCode));

            var operation = _cache[request.OperationCode];

            string result = operation.DoWork(request.Input);

            return new StringOperationResult(result);
        }
    }
}