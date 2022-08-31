using Microsoft.Xna.Framework.Input;

namespace TestGame.UserInput;

public class KeyboardInput : IUserInput
{
    private KeyboardState _previousState;
    private KeyboardState _currentState;

    public void UpdateState()
    {
        _previousState = _currentState;
        _currentState = Keyboard.GetState();
    }

    public bool IsKeyFirstPressed(Keys key)
    {
        return _previousState.IsKeyUp(key) && _currentState.IsKeyDown(key);
    }
    
    public bool IsKeyReleased(Keys key)
    {
        return _previousState.IsKeyDown(key) && _currentState.IsKeyUp(key);
    }
}