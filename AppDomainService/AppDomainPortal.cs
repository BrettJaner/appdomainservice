using System;
using System.Reflection;

namespace AppDomainService
{
    public abstract class AppDomainPortal<TRequest, TResult> : MarshalByRefObject 
    {
        protected Assembly Assembly { get; private set; }

        public override object InitializeLifetimeService() { return null; }

        protected abstract void OnAssemblyLoaded();
        protected abstract TResult Execute(TRequest request);

        internal void Load(byte[] bytes)
        {
            if (Assembly != null)
                throw new InvalidOperationException("AppDomainPortal can only be loaded once.");

            Assembly = Assembly.Load(bytes);

            OnAssemblyLoaded();
        }

        internal void LoadFile(string path)
        {
            if (Assembly != null)
                throw new InvalidOperationException("AppDomainPortal can only be loaded once.");

            Assembly = Assembly.LoadFile(path);

            OnAssemblyLoaded();
        }

        internal TResult InternalExecute(TRequest request)
        {
            if (Assembly == null)
                throw new InvalidOperationException("AppDomainPortal must call Load before any other operation.");

            return Execute(request);
        }
    }
}