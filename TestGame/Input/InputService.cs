using Microsoft.Xna.Framework.Input;

namespace TestGame.Input
{
    public class InputService
    {
        public readonly Movement Movement = new();
        public readonly Zoom Zoom = new();
        public readonly Pointer Pointer = new();
        public readonly SpecialKeys SpecialKeys = new();

        public void Update()
        {
            var keyboardState = Keyboard.GetState();
            var mouseState = Mouse.GetState();

            Movement.UpdateState(keyboardState);
            Zoom.UpdateState(mouseState);
            Pointer.UpdateState(mouseState);
            SpecialKeys.UpdateState(keyboardState);
        }
    }
}
