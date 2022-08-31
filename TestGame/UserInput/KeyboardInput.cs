using Microsoft.Xna.Framework.Input;

namespace TestGame.UserInput;

public class KeyboardInput : IUserInput
{
    private KeyboardState previousState;
    private KeyboardState currentState;

    public void UpdateState()
    {
        previousState = currentState;
        currentState = Keyboard.GetState();
    }

    public bool IsKeyFirstPressed(Keys key)
    {
        return previousState.IsKeyUp(key) && currentState.IsKeyDown(key);
    }
    
    public bool IsKeyReleased(Keys key)
    {
        return previousState.IsKeyDown(key) && currentState.IsKeyUp(key);
    }
}