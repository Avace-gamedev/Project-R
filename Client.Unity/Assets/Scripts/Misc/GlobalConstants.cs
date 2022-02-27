using System;
using Avace.Backend.Kernel.Injection;
using UnityEngine;

namespace Misc
{
    public class GlobalConstants : MonoBehaviour
    {
        public static GlobalConstants Instance;

        public static Shader TextureMapShader =>
            Instance
                ? Instance.textureMapShader
                : throw new InvalidOperationException("Global constants not initialized yet");

        [SerializeField]
        [Tooltip("Shader that is used to display a sprite from a sprite sheet.")]
        private Shader textureMapShader;

        public void Awake()
        {
            Injector.BindSingleton(this);
            Instance = Injector.Get<GlobalConstants>();
        }
    }
}