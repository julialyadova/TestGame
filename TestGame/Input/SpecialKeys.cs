using Microsoft.Xna.Framework.Input;

namespace TestGame.Input
{
    public class SpecialKeys
    {
        public const Keys Exit = Keys.Escape;
        public const Keys BuildMode = Keys.B;

        private KeyboardState _previousState;
        private KeyboardState _currentState;

        public void UpdateState(KeyboardState keyboardState)
        {
            _previousState = _currentState;
            _currentState = keyboardState;
        }

        public bool IsClicked(Keys key)
        {
            return _previousState.IsKeyUp(key) && _currentState.IsKeyDown(key);
        }
    }
}
