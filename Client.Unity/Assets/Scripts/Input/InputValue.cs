namespace Input
{
    public class InputValue
    {
        public InputType Type { get; private set; }

        public InputValue(InputType type)
        {
            Type = type;
        }
    }

    public class InputValue<T>: InputValue
    {
        public T Value { get; private set; }
        
        public InputValue(InputType type, T value) : base(type)
        {
            Value = value;
        }
    }
}