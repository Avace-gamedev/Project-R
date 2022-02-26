using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Input.Attributes;
using UnityEngine.InputSystem;

namespace Input
{
    public static class InputManager
    {
        /// <summary>
        /// These callbacks are loaded at the very beginning of the execution (see <see cref="Main"/>)
        /// </summary>
        private static readonly Dictionary<InputType, List<MethodInfo>> StaticCallbacks = new Dictionary<InputType, List<MethodInfo>>();

        /// <summary>
        /// These callbacks are set by Register
        /// </summary>
        private static readonly Dictionary<InputType, Dictionary<InputManagerCallbackId, Action<InputAction.CallbackContext>>> RegisteredCallbacks =
            new
                Dictionary<InputType, Dictionary<InputManagerCallbackId, Action<InputAction.CallbackContext>>>();

        public static void LoadCallbacks()
        {
            foreach (MethodInfo method in AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).SelectMany(t => t.GetMethods()))
            {
                if (!method.IsStatic)
                {
                    continue;
                }

                RegisterInputCallbackAttribute[] attributes = method.GetCustomAttributes().OfType<RegisterInputCallbackAttribute>().ToArray();
                if (attributes.Length == 0)
                {
                    continue;
                }

                foreach (RegisterInputCallbackAttribute attribute in attributes)
                {
                    if (!StaticCallbacks.ContainsKey(attribute.InputType))
                    {
                        StaticCallbacks.Add(attribute.InputType, new List<MethodInfo>());
                    }

                    StaticCallbacks[attribute.InputType].Add(method);
                }
            }
        }

        public static void Trigger(InputType inputType, InputAction.CallbackContext arg)
        {
            if (StaticCallbacks.ContainsKey(inputType))
            {
                foreach (MethodInfo method in StaticCallbacks[inputType])
                {
                    method.Invoke(null, new object[] { arg });
                }
            }

            if (RegisteredCallbacks.ContainsKey(inputType))
            {
                foreach (Action<InputAction.CallbackContext> action in RegisteredCallbacks[inputType].Values)
                {
                    action.Invoke(arg);
                }
            }
        }

        public static InputManagerCallbackId Register(InputType inputType, Action<InputAction.CallbackContext> callback)
        {
            if (!RegisteredCallbacks.ContainsKey(inputType))
            {
                RegisteredCallbacks.Add(inputType, new Dictionary<InputManagerCallbackId, Action<InputAction.CallbackContext>>());
            }

            InputManagerCallbackId id = new InputManagerCallbackId();
            RegisteredCallbacks[inputType].Add(id, callback);

            return id;
        }

        public static void Unregister(InputType inputType, InputManagerCallbackId id)
        {
            if (!RegisteredCallbacks.ContainsKey(inputType))
            {
                return;
            }

            if (!RegisteredCallbacks[inputType].ContainsKey(id))
            {
                return;
            }

            RegisteredCallbacks[inputType].Remove(id);
        }
    }

    [StronglyTypedId]
    public partial struct InputManagerCallbackId
    {
    }
}