using System;
using UnityEngine;

namespace Characters.Movement
{
    /// <summary>
    /// Moves an entity wrt. the given command
    /// </summary>
    public class MovementSystem: MonoBehaviour
    {
        public float maxSpeed;

        private float _speed;
        private MovementCommand? _currentCommand = null;

        public void Start()
        {
            _speed = maxSpeed;
        }
        
        public void FixedUpdate()
        {
            if (_currentCommand == null)
            {
                return;
            }

            Vector3 currentPosition = transform.position;
            Vector2 direction;
            switch (_currentCommand.Value.Type)
            {
                case MovementCommandType.Direction:
                    direction = _currentCommand.Value.Parameter;
                    break;
                case MovementCommandType.TargetPosition:
                    direction = _currentCommand.Value.Parameter - (Vector2)currentPosition;
                    break;
                default:
                    string commandType = _currentCommand.Value.Type.ToString();
                    _currentCommand = null;
                    throw new InvalidOperationException($"Unknown command type {commandType}, cancelling command");
            }

            transform.position = currentPosition + (Vector3)direction.normalized * _speed * Time.deltaTime;
        }

        public void Move(MovementCommand command)
        {
            _currentCommand = command;
        }
    }
}