using System;
using System.Collections;
using System.Collections.Generic;
using Ninject;
using Ninject.Planning.Bindings;

namespace Avace.Backend.Kernel.Injection;

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
    /// Get service that has been bound to <c>T</c> last. Return null if no service is found.
    /// </summary>
    public static T TryGet<T>()
    {
        CheckKernelIsInitialized();
        return _kernel!.TryGet<T>();
    }

    public static IEnumerable<T> GetAll<T>()
    {
        CheckKernelIsInitialized();
        return _kernel!.GetAll<T>();
    }

    /// <summary>
    /// Bind service at runtime.
    /// </summary>
    public static void Bind<TInterface, TType>() where TType : TInterface
    {
        CheckKernelIsInitialized();
        _kernel!.Bind<TInterface>().To<TType>();
    }

    /// <summary>
    /// Bind a type to a specific value.
    /// </summary>
    public static void Bind<TType>(params TType[] services)
    {
        CheckKernelIsInitialized();
        foreach (TType service in services)
        {
            _kernel!.Bind<TType>().ToConstant(service);
        }
    }

    /// <summary>
    /// Bind a type to a specific value.
    /// </summary>
    public static void Bind<TType>(TType singleton)
    {
        CheckKernelIsInitialized();
        _kernel!.Bind<TType>().ToConstant(singleton);
    }

    public static void RemoveAll<TType>()
    {
        CheckKernelIsInitialized();
        foreach (IBinding binding in _kernel!.GetBindings(typeof(TType)))
        {
            _kernel.RemoveBinding(binding);
        }
    }

    private static void CheckKernelIsInitialized()
    {
        if (_kernel == null)
        {
            throw new InvalidOperationException("Kernel has not been initialized yet");
        }
    }
}
