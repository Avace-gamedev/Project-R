using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class InputBroadcaster : MonoBehaviour
    {
        public void OnMove(InputAction.CallbackContext context)
        {
            Vector2 direction = context.ReadValue<Vector2>();
            InputManager.Trigger(new MoveInput(direction));
        }

        public void OnZoom(InputAction.CallbackContext context)
        {
            
            float diff = context.ReadValue<float>();
            Debug.Log(diff);
            InputManager.Trigger(new CameraZoomInput(diff));
        }
    }
}