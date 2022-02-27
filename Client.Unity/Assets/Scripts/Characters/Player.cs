using Avace.Backend.Kernel.Injection;
using World;

namespace Characters
{
    public class Player : Character
    {
        protected override void Start()
        {
            Injector.BindSingleton(this);
            StartPosition = Injector.Get<IPlayerStartPositionProvider>().PlayerStartPosition;

            base.Start();
        }
    }
}
