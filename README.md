appdomainservice
================
Say Hello To "The Local Web Service"

The project is designed to provide the framework and base classes required to set-up a self hosted WCF Service in a child AppDomain in order to dynamically load and unload code at runtime. Because the dynamically loaded assembly is shadow copied, the OS will not lock the file, which allows replacing the assembly during execution and reloading at will.

The project contains four assemblies. The meat and potatoes is the AppDomainService project. This is the assembly you will include in your project. It contains the base classes that make the AppDomain communication possible. The AppDomainProxy is the client side object. It holds a reference the Portal.  Because the AppDomainPortal is initialized in memory in the child AppDomain, in order for the main AppDomain to hold a reference to it, it must inherit from MarshalByObjRef.

The other three assemblies are for demonstration purposes:

The TestApplication contains console project for running the application.

The TestServiceLayer contains the Wcf Service.

The TestServiceLayer.PlugIns is the dynamically loaded assembly, that provides types for the TestServiceLayer to use.  When built, it to the c:\temp\plugins 

directory where is loaded by TestApplication. StringAppProxy specifies this location.