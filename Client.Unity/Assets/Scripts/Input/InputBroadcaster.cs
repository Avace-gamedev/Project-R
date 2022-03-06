using Misc;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class InputBroadcaster : CustomMonoBehaviour
    {
        public void OnMove(InputAction.CallbackContext context)
        {
            Vector2 direction = context.ReadValue<Vector2>();
            InputManager.Trigger(new MoveInput(direction));
        }

        public void OnZoom(InputAction.CallbackContext context)
        {
            
            float diff = context.ReadValue<float>();
            InputManager.Trigger(new CameraZoomInput(diff));
        }
    }
}