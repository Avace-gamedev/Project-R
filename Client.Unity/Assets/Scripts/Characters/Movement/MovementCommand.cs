using UnityEngine;

namespace Characters.Movement
{
    public struct MovementCommand
    {
        public MovementCommandType Type { get; private set; }
        public Vector2 Parameter { get; private set; }

        public static MovementCommand Direction(Vector2 direction)
        {
            return new MovementCommand
            {
                Type = MovementCommandType.Direction,
                Parameter = direction,
            };
        }
        
        public static MovementCommand TargetPosition(Vector2 position)
        {
            return new MovementCommand
            {
                Type = MovementCommandType.TargetPosition,
                Parameter = position,
            };
        }
    }

    public enum MovementCommandType
    {
        Direction,
        TargetPosition,
    }
}