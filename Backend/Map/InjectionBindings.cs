using Avace.Backend.Interfaces.Map;
using Ninject.Modules;

namespace Avace.Backend.Map
{
    public class InjectionBindings : NinjectModule
    {
        public override void Load()
        {
            Bind<IMapProvider>().ToConstant(new DevMapProvider());
        }
    }
}