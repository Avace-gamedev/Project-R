using Avace.Backend.Kernel.Injection;
using World;

namespace Characters
{
    public class Player : Character
    {
        protected override void RegisterInjectionBindings()
        {
            base.RegisterInjectionBindings();
            Injector.BindSingleton(this);
        }

        protected override void UnregisterInjectionBindings()
        {
            Injector.RemoveAll<Player>();
        }

        protected override void Start()
        {
            StartPosition = Injector.Get<IPlayerStartPositionProvider>().PlayerStartPosition;

            base.Start();
        }
    }
}
