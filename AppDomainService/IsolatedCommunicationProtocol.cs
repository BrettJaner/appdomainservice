using System;
using System.Threading;

namespace AppDomainService
{
    internal class IsolatedCommunicationProtocol : ICommunicationProtocol
    {
        private AppDomain _appDomain;

        public bool IsPortalRemote { get { return true; } }

        public TPortal GetPortal<TPortal, TRequest, TResult>(string assemblyFilePath, byte[] assemblyBytes) 
            where TPortal : AppDomainPortal<TRequest, TResult>
        {
            _appDomain = AppDomain.CreateDomain(Guid.NewGuid().ToString(), AppDomain.CurrentDomain.Evidence, AppDomain.CurrentDomain.SetupInformation);
            _appDomain.SetThreadPrincipal(Thread.CurrentPrincipal);

            var tempPortal = (TPortal)_appDomain.CreateInstanceAndUnwrap(typeof(TPortal).Assembly.FullName, typeof(TPortal).FullName);

            #if DEBUG
                tempPortal.LoadFile(assemblyFilePath);
            #else
                tempPortal.Load(assemblyBytes);
            #endif   

            return tempPortal;
        }

        public void Reset()
        {
            if (_appDomain != null)
            {
                try
                {
                    AppDomain.Unload(_appDomain);
                }
                finally
                {
                    _appDomain = null;
                }
            }
        }
    }
}