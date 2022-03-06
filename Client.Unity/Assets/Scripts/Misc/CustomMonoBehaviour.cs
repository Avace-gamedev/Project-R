using Avace.Backend.Interfaces.Logging;
using Avace.Backend.Kernel.Injection;
using UnityEngine;

namespace Misc
{
    /// <summary>
    /// Provides basic tools that behaviours might need such as hooks to register injection bindings
    /// at the right moment (<see cref="RegisterInjectionBindings"/> and <see cref="UnregisterInjectionBindings"/>), a <see cref="Logger"/>, etc..
    /// </summary>
    public abstract class CustomMonoBehaviour : MonoBehaviour
    {
        protected static ICustomLogger Logger;

        /// <summary>
        /// This method will be called in OnEnable. It should register all the injection bindings that are provided by the inheritor.
        /// </summary>
        protected virtual void RegisterInjectionBindings()
        {
        }

        /// <summary>
        /// This method will be called in OnDisable. It should unregister all the injection bindings that it has registered
        /// in <see cref="RegisterInjectionBindings"/>
        /// </summary>
        protected virtual void UnregisterInjectionBindings()
        {
        }

        protected void OnEnable()
        {
            RegisterInjectionBindings();
            Logger = Injector.Get<ILoggerProvider>().GetLogger(GetType().Name);
        }

        protected void OnDisable()
        {
            UnregisterInjectionBindings();
        }
    }
}