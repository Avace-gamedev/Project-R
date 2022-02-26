using System;
using Ninject;

namespace Avace.Backend.Kernel.Injection
{
    public static class Injector
    {
        private static IKernel? _kernel;

        /// <summary>
        /// Initialize dependency injection. This MUST be called before any other method.
        /// </summary>
        public static void Initialize()
        {
            _kernel = new StandardKernel();
            _kernel.Load(AppDomain.CurrentDomain.GetAssemblies());
        }

        /// <summary>
        /// Get service that has been bound to <c>T</c> last.
        /// </summary>
        public static T Get<T>()
        {
            CheckKernelIsInitialized();
            return _kernel!.Get<T>();
        }

        /// <summary>
        /// Bind service at runtime.
        /// </summary>
        public static void Bind<TInterface, TType>() where TType : TInterface
        {
            CheckKernelIsInitialized();
            _kernel!.Bind<TInterface>().To<TType>();
        }

        private static void CheckKernelIsInitialized()
        {
            if (_kernel == null)
            {
                throw new InvalidOperationException("Kernel has not been initialized yet");
            }
        }
    }
}