using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;

namespace AppDomainService
{
    internal class LocalCommunicationProtocol : ICommunicationProtocol
    {
        private static readonly ConcurrentDictionary<string, PortalCache> LoadedPortalsByAssemblyName = new ConcurrentDictionary<string, PortalCache>();

        public bool IsPortalRemote { get { return false; } }

        public TPortal GetPortal<TPortal, TRequest, TResult>(string assemblyFilePath, byte[] assemblyBytes) 
            where TPortal : AppDomainPortal<TRequest, TResult>
        {
            var portalCache = LoadedPortalsByAssemblyName.GetOrAdd(assemblyFilePath, key =>
            {
                var tempPortal = (TPortal)AppDomain.CurrentDomain.CreateInstanceAndUnwrap(typeof(TPortal).Assembly.FullName, typeof(TPortal).FullName);

                #if DEBUG
                    tempPortal.LoadFile(assemblyFilePath);
                #else
                    tempPortal.Load(assemblyBytes);
                #endif

                return new PortalCache(assemblyBytes, tempPortal);
            });

            if (!Enumerable.SequenceEqual(portalCache.LoadedAssemblyBytes, assemblyBytes))
                throw new DynamicAssemblyLoadException(string.Format("Older version of {0} is loaded, restart is required.", Path.GetFileName(assemblyFilePath)));

            return (TPortal)portalCache.Portal;
        }

        public void Reset() { }

        private class PortalCache
        {
            internal readonly byte[] LoadedAssemblyBytes;
            internal readonly MarshalByRefObject Portal;

            internal PortalCache(byte[] loadedAssemblyBytes, MarshalByRefObject portal)
            {
                LoadedAssemblyBytes = loadedAssemblyBytes;
                Portal = portal;
            }
        }
    }
}