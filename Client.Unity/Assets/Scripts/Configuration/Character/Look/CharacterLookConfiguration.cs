using Characters;
using Characters.Enums;
using UnityEngine;

namespace Configuration.Character.Look
{
    public abstract class CharacterLookConfiguration : ScriptableObject
    {
        public Texture2D spriteSheet;

        [Tooltip("Size of one sprite in the sprite sheet (in pixels)")]
        public Vector2Int spriteSize = new Vector2Int(32, 32);

        [Tooltip("Position of pivot for each sprite in the sprite sheet (normalized)")]
        public Vector2 pivot = new Vector2(0.5f, 0);

        [Range(0.1f, 10f)]
        [Tooltip("Height of the character sprite, its width will be computed to match the ratio defined by spriteSize.")]
        public float sizeModifier = 1;

        [Tooltip("Global multiplicator that should be applied to the speed of all the animations of this character.")]
        public float animationSpeedModifier = 1;

        /// <summary>
        /// Index of frames in order per animation 
        /// </summary>
        public abstract int[] GetAnimationFrames(CharacterState state, CharacterOrientation orientation);

        /// <summary>
        /// Animation speed per animation, in frames/sec
        /// </summary>
        public float GetAnimationSpeed(CharacterState state, CharacterOrientation orientation)
        {
            return GetAnimationSpeedInternal(state, orientation) * animationSpeedModifier;
        }

        protected abstract float GetAnimationSpeedInternal(CharacterState state, CharacterOrientation orientation);
    }
}