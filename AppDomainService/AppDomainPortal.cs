using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Xml;

namespace AppDomainService
{
    public class AppDomainPortal<TContract, TService> : MarshalByRefObject
    {
        private static ServiceHost Host;
        private static object SyncLock = new object();

        static AppDomainPortal()
        {
            AppDomain.CurrentDomain.DomainUnload += AppDomain_OnDomainUnload;
        }

        public override object InitializeLifetimeService() { return null; }

        internal void LoadFrom(string plugInPath)
        {
            if (plugInPath == null)
                throw new ArgumentNullException("plugInPath");


            foreach (var fileInfo in Directory.GetFiles(plugInPath)
                                              .Select(x => new FileInfo(x))
                                              .Where(x => string.Equals(x.Extension, ".dll", StringComparison.OrdinalIgnoreCase)))
            {
                Assembly.LoadFrom(fileInfo.FullName);
            }
        }

        internal void Start()
        {
            lock (SyncLock)
            {
                if (Host != null)
                    throw new AppDomainServiceException("WcfSelfHost Service is already started");

                Host = GetServiceHost();
                Host.Open();
            }
        }

        protected virtual ServiceHost GetServiceHost()
        {
            var host = new ServiceHost(typeof(TService), new Uri("net.pipe://localhost"));

            host.AddServiceEndpoint(typeof(TContract),
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
                },
                string.Format("{0}_{1}", typeof(TContract).FullName, typeof(TService).FullName));

            var debugBehavior = host.Description.Behaviors.OfType<ServiceDebugBehavior>().SingleOrDefault();

            if (debugBehavior != null)
                debugBehavior.IncludeExceptionDetailInFaults = true;
            else
                host.Description.Behaviors.Add(new ServiceDebugBehavior { IncludeExceptionDetailInFaults = true });

            return host;
        }

        private static void AppDomain_OnDomainUnload(object sender, EventArgs args)
        {
            try
            {
                if (Host != null)
                    Host.Close();
            }
            catch (Exception)
            {
                if (Host != null)
                    Host.Abort();

                throw;
            }
        }
    }
}