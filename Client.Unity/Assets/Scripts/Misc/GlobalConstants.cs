using System;
using System.Reflection;
using Avace.Backend.Interfaces.Logging;
using Avace.Backend.Kernel.Injection;
using UnityEngine;

namespace Misc
{
    public class GlobalConstants : CustomMonoBehaviour
    {
        public static GlobalConstants Instance => Injector.TryGet<GlobalConstants>();

        [Tooltip("Shader that is used to display a sprite from a sprite sheet.")]
        public Shader textureMapShader;

        protected override void RegisterInjectionBindings()
        {
            Injector.BindSingleton(this);
        }

        protected override void UnregisterInjectionBindings()
        {
            Injector.RemoveAll<GlobalConstants>();
        }

        public static GlobalConstants Get()
        {
            return Instance ? Instance : throw new InvalidOperationException("Global constants not initialized yet");
        }
    }
}