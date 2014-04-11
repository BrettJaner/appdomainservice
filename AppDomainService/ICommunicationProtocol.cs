namespace AppDomainService
{
    internal interface ICommunicationProtocol
    {
        bool IsPortalRemote { get; }
        void Reset();

        TPortal GetPortal<TPortal, TRequest, TResult>(string assemblyFilePath, byte[] assemblyBytes)
            where TPortal : AppDomainPortal<TRequest, TResult>;
    }
}
