﻿using System;
using Ninject;

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
    public static void BindSingleton<TType>(TType singleton)
    {
        CheckKernelIsInitialized();
        _kernel!.Bind<TType>().ToConstant(singleton);
    }

    private static void CheckKernelIsInitialized()
    {
        if (_kernel == null)
        {
            throw new InvalidOperationException("Kernel has not been initialized yet");
        }
    }
}