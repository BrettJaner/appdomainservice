using AppDomainService;

namespace MyTestApplication
{
    public class StringAppProxy : AppDomainProxy<IStringOperationPortal, StringOperationPortal>
    {
        public StringAppProxy() : base(@"c:\temp\hotswap\") { }
    }
}