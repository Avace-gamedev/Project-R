using System;
using Avace.Backend.Kernel.Injection;
using Characters.Enums;
using Characters.Look;
using Characters.Movement;
using Input;
using Input.Attributes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters
{
    /// <summary>
    /// Connects player to inputs
    /// </summary>
    public static class PlayerInputManager
    {
        private static Player _player;

        [RegisterInputCallback(InputType.Move)]
        public static void OnMove(InputAction.CallbackContext context)
        {
            GetPlayerComponentsIfNecessary();
            
            Vector2 direction = context.ReadValue<Vector2>();

            _player.Move(direction);
        }

        private static void GetPlayerComponentsIfNecessary()
        {
            _player = Injector.TryGet<Player>();
            if (_player == null)
            {
                throw new InvalidOperationException($"Could not get player");
            }
        }
    }
}