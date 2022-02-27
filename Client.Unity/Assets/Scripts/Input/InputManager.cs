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
        private static readonly Dictionary<InputType, Dictionary<InputManagerCallbackId, Action<InputValue>>> RegisteredCallbacks =
            new
                Dictionary<InputType, Dictionary<InputManagerCallbackId, Action<InputValue>>>();

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

        public static void Trigger(InputValue arg)
        {
            if (StaticCallbacks.ContainsKey(arg.Type))
            {
                foreach (MethodInfo method in StaticCallbacks[arg.Type])
                {
                    method.Invoke(null, new object[] { arg });
                }
            }

            if (RegisteredCallbacks.ContainsKey(arg.Type))
            {
                foreach (Action<InputValue> action in RegisteredCallbacks[arg.Type].Values)
                {
                    action.Invoke(arg);
                }
            }
        }

        public static InputManagerCallbackId Register<T>(InputType inputType, Action<T> callback) where T : InputValue
        {
            if (!RegisteredCallbacks.ContainsKey(inputType))
            {
                RegisteredCallbacks.Add(inputType, new Dictionary<InputManagerCallbackId, Action<InputValue>>());
            }

            InputManagerCallbackId id = new InputManagerCallbackId();
            RegisteredCallbacks[inputType].Add(id, (Action<InputValue>)callback);

            return id;
        }

        public static void Unregister(InputManagerCallbackId id)
        {
            foreach (InputType type in RegisteredCallbacks.Keys.Where(type => RegisteredCallbacks[type].ContainsKey(id)))
            {
                RegisteredCallbacks[type].Remove(id);
                return;
            }
        }
    }

    [StronglyTypedId]
    public partial struct InputManagerCallbackId
    {
    }
}