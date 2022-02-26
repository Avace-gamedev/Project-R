using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class InputBroadcaster : MonoBehaviour
    {
        public void OnMove(InputAction.CallbackContext context)
        {
            InputManager.Trigger(InputType.Move, context);
        }
    }
}
