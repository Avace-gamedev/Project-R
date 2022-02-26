using System;
using Avace.Backend.Kernel.Injection;
using Input;
using Input.Attributes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Movement
{
    /// <summary>
    /// Connects player's movement system to inputs
    /// </summary>
    public class PlayerMovementSystem
    {
        private static MovementSystem _playerMovementSystem;

        [RegisterInputCallback(InputType.Move)]
        public static void OnMove(InputAction.CallbackContext context)
        {
            GetPlayerMovementSystemIfNecessary();

            _playerMovementSystem.Move(MovementCommand.Direction(context.ReadValue<Vector2>()));
        }

        private static void GetPlayerMovementSystemIfNecessary()
        {
            if (_playerMovementSystem == null)
            {
                _playerMovementSystem = Injector.TryGet<Player>()?.GetComponent<MovementSystem>();
            }

            if (_playerMovementSystem == null)
            {
                throw new InvalidOperationException("Could not get player movement system");
            }
        }
    }
}