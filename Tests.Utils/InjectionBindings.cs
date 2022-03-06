using Avace.Backend.Interfaces.Logging;
using Ninject.Modules;

namespace Tests.Utils
{
    public class InjectionBindings : NinjectModule
    {
        public override void Load()
        {
            Bind<ICustomLoggerProvider>().To<CustomLoggerProviderForTest>();
        }
    }
}