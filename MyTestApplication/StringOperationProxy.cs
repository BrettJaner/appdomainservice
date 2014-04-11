using System.Configuration;
using System.IO;
using AppDomainService;

namespace MyTestApplication
{
    public class StringOperationProxy : AppDomainProxy<StringOperationPortal, StringOperationRequest, StringOperationResult>
    {
        protected override string DynamicallyLoadedAssemblyPath
        {
            get
            {
                if (ConfigurationManager.AppSettings["hotswapdirectory"] == null)
                    throw new ConfigurationErrorsException("hotswapdirectory can not be found in app.config/web.config.");

                return Path.Combine(ConfigurationManager.AppSettings["hotswapdirectory"], "DynamicallyLoadedAssembly.dll");
            }
        }
    }
}
