using System;
using System.Configuration;
using System.IO;
using System.Threading;

namespace AppDomainService
{
    public abstract class AppDomainProxy<TPortal, TRequest, TResult>
        where TPortal : AppDomainPortal<TRequest, TResult>
    {
        static AppDomainProxy()
        {
            string appConfig = ConfigurationManager.AppSettings["AppDomainIsolationLevel"] ?? "IsolatedAppDomain";

            var appDomainIsolationLevel = (AppDomainIsolationLevels)Enum.Parse(typeof(AppDomainIsolationLevels), appConfig);

            switch (appDomainIsolationLevel)
            {
                case AppDomainIsolationLevels.IsolatedAppDomain:
                    _protocol = new IsolatedCommunicationProtocol();
                    break;

                case AppDomainIsolationLevels.CurrentAppDomain:
                    _protocol = new LocalCommunicationProtocol();
                    break;

                default:
                    throw new InvalidOperationException("Isolation Level is not associated with a Protocol.");
            }
        }

        private enum AppDomainIsolationLevels
        {
            IsolatedAppDomain,
            CurrentAppDomain
        }

        private static readonly ICommunicationProtocol _protocol;

        private static readonly object _padLock = new object();
        private static TPortal _portal;

        protected internal abstract string DynamicallyLoadedAssemblyPath { get; }

        public bool IsPortalRemote { get { return _protocol.IsPortalRemote; } }

        public TResult ExecuteRequest(TRequest dto)
        {
            var portal = GetPortal();

            return portal.InternalExecute(dto);
        }

        private TPortal GetPortal()
        {
            if (_portal == null)
            {
                lock (_padLock)
                {
                    if (_portal == null)
                    {
                        var assemblyBytes = GetAssemblyBytes();

                        _portal = _protocol.GetPortal<TPortal, TRequest, TResult>(DynamicallyLoadedAssemblyPath, assemblyBytes);
                    }
                }
            }

            return _portal;
        }

        public void InvalidateAppDomain()
        {
            lock (_padLock)
            {
                _portal = null;
                _protocol.Reset();
            }
        }

        private byte[] GetAssemblyBytes()
        {
            if (!File.Exists(DynamicallyLoadedAssemblyPath))
                throw new InvalidOperationException(string.Format("Dynamically Loaded Assembly does not exist ({0})", DynamicallyLoadedAssemblyPath));

            return File.ReadAllBytes(DynamicallyLoadedAssemblyPath);
        }
    }
}