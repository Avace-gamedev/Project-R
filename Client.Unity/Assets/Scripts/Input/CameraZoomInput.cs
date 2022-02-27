namespace Input
{
    public class CameraZoomInput: InputValue<float>
    {
        public CameraZoomInput(float value) : base(InputType.CameraZoom, value)
        {
        }
    }
}