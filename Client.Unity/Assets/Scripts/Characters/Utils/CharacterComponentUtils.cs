using System;
using Configuration.Character;
using UnityEngine;

namespace Characters.Utils
{
    public static class CharacterComponentUtils
    {
        public static T GetConfigurationFromCharacterConfiguration<T>(this GameObject gameObject, Func<CharacterConfiguration, T> getConfiguration)
            where T : ScriptableObject
        {
            Player player = gameObject.GetComponent<Player>();
            return player != null
                ? player.configuration != null
                    ? getConfiguration.Invoke(player.configuration)
                    : null
                : null;
        }

        public static TComponent RequireComponent<TComponent>(this GameObject gameObject) where TComponent : MonoBehaviour
        {
            TComponent result = gameObject.GetComponent<TComponent>();
            if (result == null)
            {
                throw new InvalidOperationException($"Could not find component {typeof(TComponent)}");
            }

            return result;
        }
    }
}