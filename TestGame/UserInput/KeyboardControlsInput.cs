using Microsoft.Xna.Framework.Input;

namespace TestGame.UserInput;

public class KeyboardControlsInput : IControlsInput
{
    private Control _interact;
    public void UpdateState()
    {
        var kstate = Keyboard.GetState();
        _interact.UpdateState(kstate.IsKeyDown(Keys.E));
    }

    public bool ControlPressed()
    {
        return false;
    }

    public GameControl GetPressedControl()
    {
        return GameControl.None;
    }
}