using Configuration.Character.Look;
using Configuration.Character.Movement;
using UnityEngine;

namespace Configuration.Character
{
    [CreateAssetMenu(fileName = "CharacterConfiguration", menuName = "Configuration/Character")]
    public class CharacterConfiguration : ScriptableObject
    {
        public CharacterLookConfiguration look;
        public CharacterMovementConfiguration movement;
    }
}