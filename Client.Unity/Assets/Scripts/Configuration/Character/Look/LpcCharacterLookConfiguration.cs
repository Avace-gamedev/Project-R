using System;
using System.Linq;
using Characters;
using Characters.Enums;
using UnityEngine;

namespace Configuration.Character.Look
{
    /// <summary>
    /// Implements the character look configuration for LPC sheets.
    /// </summary>
    [CreateAssetMenu(fileName = "LpcCharacterLookConfiguration", menuName = "Configuration/Character Look (LPC)")]
    public class LpcCharacterLookConfiguration : CharacterLookConfiguration
    {
        [Tooltip("Frame rate in frames/sec")]
        public float frameRate = 25f;
        
        public override int[] GetAnimationFrames(CharacterState state, CharacterOrientation orientation)
        {
            state = FallbackIfRequired(state);
            
            int startPosition = GetAnimationStartPosition(state, orientation);
            int length = GetAnimationLength(state, orientation);

            return Enumerable.Range(startPosition, length).ToArray();
        }

        protected override float GetAnimationSpeedInternal(CharacterState state, CharacterOrientation orientation)
        {
            return frameRate;
        }

        private static CharacterState FallbackIfRequired(CharacterState state)
        {
            switch (state)
            {
                case CharacterState.Idle:
                case CharacterState.Walking:
                    return state;
                case CharacterState.Running:
                    return CharacterState.Idle;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, "Unknown animation");
            }
        }

        private static int GetAnimationStartPosition(CharacterState state, CharacterOrientation orientation)
        {
            return state switch
            {
                CharacterState.Idle => orientation switch
                {
                    CharacterOrientation.North => 0,
                    CharacterOrientation.West => 13,
                    CharacterOrientation.South => 26,
                    CharacterOrientation.East => 39,
                    _ => throw new ArgumentOutOfRangeException(nameof(orientation), orientation, "Unknown orientation")
                },
                CharacterState.Walking => orientation switch
                {
                    CharacterOrientation.North => 104,
                    CharacterOrientation.West => 117,
                    CharacterOrientation.South => 130,
                    CharacterOrientation.East => 143,
                    _ => throw new ArgumentOutOfRangeException(nameof(orientation), orientation, "Unknown orientation")
                },
                CharacterState.Running => throw new NotSupportedException($"Animation {state} not supported"),
                _ => throw new ArgumentOutOfRangeException(nameof(state), state, "Unknown animation")
            };
        }

        private static int GetAnimationLength(CharacterState state, CharacterOrientation orientation)
        {
            return state switch
            {
                CharacterState.Idle => 1,
                CharacterState.Walking => 9,
                CharacterState.Running => throw new NotSupportedException($"Animation {state} not supported"),
                _ => throw new ArgumentOutOfRangeException(nameof(state), state, "Unknown animation")
            };
        }
    }
}