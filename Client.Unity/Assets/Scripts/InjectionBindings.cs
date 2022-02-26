using Avace.Backend.Interfaces.Logging;
using Logging;
using Ninject.Modules;

public class InjectionBindings: NinjectModule
{
    public override void Load()
    {
        Bind<ILoggerProvider>().To<UnityLoggerProvider>();
    }
}