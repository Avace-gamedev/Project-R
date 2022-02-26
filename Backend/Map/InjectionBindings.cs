using Avace.Backend.Interfaces.Map;
using Ninject.Modules;

namespace Avace.Backend.Map
{
    internal class InjectionBindings : NinjectModule
    {
        public override void Load()
        {
            Bind<IMapProvider>().To<DevMapProvider>();
        }
    }
}