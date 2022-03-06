using System;
using System.Collections.Generic;
using System.Linq;
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
        InitializeKernelIfNecessary();
        return _kernel!.Get<T>();
    }

    /// <summary>
    /// Get all service that have been bound to <c>T</c>.
    /// </summary>
    public static IEnumerable<T> GetAll<T>()
    {
        InitializeKernelIfNecessary();
        return _kernel!.GetAll<T>();
    }

    /// <summary>
    /// Get service that has been bound to <c>T</c> last. Return null if no service is found.
    /// </summary>
    public static T TryGet<T>()
    {
        InitializeKernelIfNecessary();
        return _kernel!.TryGet<T>();
    }

    /// <summary>
    /// Bind service at runtime.
    /// </summary>
    public static void Bind<TInterface, TType>() where TType : TInterface
    {
        InitializeKernelIfNecessary();
        _kernel!.Bind<TInterface>().To<TType>();
    }

    /// <summary>
    /// Bind a type to a specific value.
    /// </summary>
    public static void BindSingleton<TType>(TType singleton)
    {
        InitializeKernelIfNecessary();
        if (Any<TType>())
        {
            RemoveAll<TType>();
        }

        _kernel!.Bind<TType>().ToConstant(singleton);
    }

    private static bool Any<TType>()
    {
        InitializeKernelIfNecessary();
        return _kernel!.GetBindings(typeof(TType)).Any();
    }

    public static void RemoveAll<TType>()
    {
        InitializeKernelIfNecessary();
        foreach (IBinding binding in _kernel!.GetBindings(typeof(TType)))
        {
            _kernel.RemoveBinding(binding);
        }
    }
    
    public static void RemoveAll(Type type)
    {
        InitializeKernelIfNecessary();
        foreach (IBinding binding in _kernel!.GetBindings(type))
        {
            _kernel.RemoveBinding(binding);
        }
    }

    private static void InitializeKernelIfNecessary()
    {
        if (_kernel == null)
        {
            Initialize();
        }
    }
}