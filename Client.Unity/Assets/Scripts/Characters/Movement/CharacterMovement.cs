using System;
using Characters.Utils;
using Configuration.Character.Movement;
using UnityEngine;

namespace Characters.Movement
{
    /// <summary>
    /// Moves an entity wrt. the given command
    /// </summary>
    public class CharacterMovement : MonoBehaviour
    {
        public CharacterMovementConfiguration movementConfiguration;

        private Character _character;

        public void Start()
        {
            _character = gameObject.RequireComponent<Character>();
        }

        public void FixedUpdate()
        {
            if (_character.CurrentMovementCommand == null)
            {
                return;
            }

            Vector3 currentPosition = transform.position;
            Vector2 direction = _character.CurrentMovementCommand.Value.GetDirection(currentPosition);

            transform.position = currentPosition + _character.CurrentMovementSpeed * Time.deltaTime * (Vector3)direction.normalized;
        }

        private void OnValidate()
        {
            if (movementConfiguration == null)
            {
                movementConfiguration = gameObject.GetConfigurationFromCharacterConfiguration(config => config.movement);
            }
        }
    }
}