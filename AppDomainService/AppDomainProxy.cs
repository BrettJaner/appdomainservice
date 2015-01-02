using System;
using System.Collections.Concurrent;
using System.ServiceModel;
using System.Threading;
using System.Xml;

namespace AppDomainService
{
    public class AppDomainProxy<TContract, TService>
        where TService : TContract
    {
        private static readonly ConcurrentDictionary<Type, AppDomain> AppDomainsByTypes = new ConcurrentDictionary<Type, AppDomain>();

        private readonly string _plugInPath;

        protected virtual Type AppDomainPortalType { get { return typeof(AppDomainPortal<TContract, TService>); } }

        public AppDomainProxy() { }

        protected AppDomainProxy(string plugInPath)
        {
            _plugInPath = plugInPath;
        }

        public void Reset()
        {
            AppDomain domain;

            if (AppDomainsByTypes.TryRemove(GetType(), out domain))
                AppDomain.Unload(domain);
        }

        public ChannelFactory<TContract> CreateChannelFactory()
        {
            StartUpServiceIfDown();

            return InitializeChannelFactory();
        }

        private void StartUpServiceIfDown()
        {
            AppDomainsByTypes.GetOrAdd(GetType(), type =>
            {
                var domain = AppDomain.CreateDomain(Guid.NewGuid().ToString(),
                    AppDomain.CurrentDomain.Evidence,
                    new AppDomainSetup
                    {
                        ShadowCopyDirectories = _plugInPath,
                        ShadowCopyFiles = "true",
                    });

                domain.UnhandledException += AppDomain_UnhandledException;
                domain.SetThreadPrincipal(Thread.CurrentPrincipal);

                var host = (AppDomainPortal<TContract, TService>)domain.CreateInstanceAndUnwrap(AppDomainPortalType.Assembly.FullName, AppDomainPortalType.FullName);
                host.LoadFrom(_plugInPath);

                host.Start();

                return domain;
            });
        }

        protected virtual ChannelFactory<TContract> InitializeChannelFactory()
        {
            return new ChannelFactory<TContract>(
                new NetNamedPipeBinding
                {
                    ReceiveTimeout = TimeSpan.FromMinutes(10),
                    SendTimeout = TimeSpan.FromMinutes(10),
                    MaxBufferSize = int.MaxValue,
                    MaxBufferPoolSize = int.MaxValue,
                    MaxReceivedMessageSize = int.MaxValue,
                    ReaderQuotas = new XmlDictionaryReaderQuotas
                    {
                        MaxDepth = int.MaxValue,
                        MaxStringContentLength = int.MaxValue,
                        MaxArrayLength = int.MaxValue,
                        MaxBytesPerRead = int.MaxValue,
                        MaxNameTableCharCount = int.MaxValue
                    },
                    Security = new NetNamedPipeSecurity
                    {
                        Mode = NetNamedPipeSecurityMode.None
                    }
                }, new EndpointAddress(string.Format("net.pipe://localhost/{0}_{1}", typeof(TContract).FullName, typeof(TService).FullName)));
        }

        private static void AppDomain_UnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            var exception = args.ExceptionObject as Exception;

            if (exception != null)
                throw new AppDomainServiceException("Exception happened in another AppDomain.  Please see inner exception.", exception);

            throw new AppDomainServiceException(string.Format("Exception happened in another AppDomain:  {0}", args.ExceptionObject));
        }
    }
}