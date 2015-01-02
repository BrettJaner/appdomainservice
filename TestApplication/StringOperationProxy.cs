using AppDomainService;
using TestServiceLayer;

namespace TestApplication
{
    public class StringAppProxy : AppDomainProxy<IStringOperationPortal, StringOperationPortal>
    {
        public StringAppProxy() : base(@"c:\temp\plugin\") { }
    }
}