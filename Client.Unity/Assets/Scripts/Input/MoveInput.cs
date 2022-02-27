using UnityEngine;

namespace Input
{
    public class MoveInput: InputValue<Vector2>
    {
        public MoveInput(Vector2 value) : base(InputType.Move, value)
        {
        }
    }
}