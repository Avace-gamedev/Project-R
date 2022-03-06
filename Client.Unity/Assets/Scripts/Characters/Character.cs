using System;
using System.Reflection;
using Avace.Backend.Interfaces.Logging;
using Avace.Backend.Kernel.Injection;
using Characters.Enums;
using Characters.Look;
using Characters.Movement;
using Configuration.Character;
using Misc;
using UnityEngine;
using World;

namespace Characters
{
    /// <summary>
    /// Implements a state machine for handling character states.  
    /// </summary>
    [RequireComponent(typeof(CharacterMovement))]
    [RequireComponent(typeof(CharacterLook))]
    public class Character : CustomMonoBehaviour
    {
        public CharacterConfiguration configuration;

        [Header("Initial state (movement)")]
        public CharacterState initialState = CharacterState.Idle;
        public CharacterOrientation initialOrientation = CharacterOrientation.South;
        
        // Public interface
        // These properties are used by character component 
        // e.g. CharacterLook uses State and Orientation to display the correct animation
        // These properties sum up the character's transient state

        public CharacterState State { get; private set; }
        public CharacterOrientation Orientation { get; private set; }
        public float CurrentMovementSpeed { get; private set; }
        public MovementCommand? CurrentMovementCommand { get; private set; }

        /// <summary>
        /// Set the starting cell of the character before calling Start() 
        /// </summary>
        protected Vector2Int StartPosition;

        protected virtual void Start()
        {
            transform.position = Injector.Get<ICoordinatesConverter>().Convert(StartPosition);

            State = initialState;
            Orientation = initialOrientation;
        }

        public void Move(Vector2 direction)
        {
            if (direction == Vector2.zero)
            {
                StopMoving();
            }
            else
            {
                CurrentMovementCommand = MovementCommand.Direction(direction);
            }
        }

        public void MoveTo(Vector2 target)
        {
            CurrentMovementCommand = MovementCommand.TargetPosition(target);
        }

        public void StopMoving()
        {
            CurrentMovementCommand = null;
        }

        private void Update()
        {
            CurrentMovementSpeed = configuration.movement.maxSpeed;
            
            UpdateMovementCommand();
            UpdateState();
            UpdateOrientation();
        }

        private void UpdateMovementCommand()
        {
            if (CurrentMovementCommand == null)
            {
                return;
            }

            switch (CurrentMovementCommand.Value.Type)
            {
                case MovementCommandType.TargetPosition:
                    float dist = Vector2.Distance(CurrentMovementCommand.Value.Parameter, transform.position);
                    if (dist < 0.01f)
                    {
                        StopMoving();
                    }

                    break;
                case MovementCommandType.Direction:
                default:
                    break;
            }
        }

        private void UpdateState()
        {
            switch (State)
            {
                case CharacterState.Idle:
                    if (CurrentMovementCommand != null)
                    {
                        State = CharacterState.Walking;
                    }

                    break;
                case CharacterState.Walking:
                    if (CurrentMovementCommand == null)
                    {
                        State = CharacterState.Idle;
                    }

                    break;
                case CharacterState.Running:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void UpdateOrientation()
        {
            Vector2 direction = CurrentMovementCommand?.GetDirection(transform.position) ?? Vector2.zero;

            if (direction == Vector2.zero)
            {
                return;
            }

            if (Math.Abs(direction.x) > Math.Abs(direction.y))
            {
                Orientation = direction.x < 0 ? CharacterOrientation.West : CharacterOrientation.East;
            }
            else
            {
                Orientation = direction.y < 0 ? CharacterOrientation.South : CharacterOrientation.North;
            }
        }

        private void OnValidate()
        {
            if (configuration == null)
            {
                return;
            }

            CharacterLook characterLook = gameObject.GetComponent<CharacterLook>();
            if (characterLook && characterLook.characterLookConfiguration == null)
            {
                characterLook.characterLookConfiguration = configuration.look;
            }

            CharacterMovement characterMovement = gameObject.GetComponent<CharacterMovement>();
            if (characterMovement && characterMovement.movementConfiguration == null)
            {
                characterMovement.movementConfiguration = configuration.movement;
            }
        }
    }
}