using UnityEngine;

namespace Configuration.Character.Movement
{
    [CreateAssetMenu(fileName = "CharacterMovementConfiguration", menuName = "Configuration/Character Movement")]
    public class CharacterMovementConfiguration: ScriptableObject
    {
        [Header("Parameters")]
        public float maxSpeed = 5;
        
        [Header("Features")]
        public bool canMove = true;
        public bool canRun = true;
    }
}