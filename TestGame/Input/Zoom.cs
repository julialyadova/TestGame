using Microsoft.Xna.Framework.Input;

namespace TestGame.Input
{
    public class Zoom
    {
        private const float ZoomSpeed = 0.0005f;

        private int _currentScroll;
        private int _previousScroll;

        public bool IsZooming { get; private set; }
        public float Value { get; private set; }

        public void UpdateState(MouseState mouseState)
        {
            _previousScroll = _currentScroll;
            _currentScroll = mouseState.ScrollWheelValue;

            Value = (_currentScroll - _previousScroll) * ZoomSpeed;
            IsZooming = Value != 0;
        }
    }
}
