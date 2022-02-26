using System;

namespace Input.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class RegisterInputCallbackAttribute : Attribute
    {
        public InputType InputType { get; }

        public RegisterInputCallbackAttribute(InputType inputType)
        {
            InputType = inputType;
        }
    }
}