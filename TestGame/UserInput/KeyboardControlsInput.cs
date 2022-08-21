using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TestGame.UserInput;

public class KeyboardControlsInput : ControlsInput
{
    private bool _pressedE;
    public override void Update(GameTime gameTime)
    {
        var kstate = Keyboard.GetState();

        if (kstate.IsKeyDown(Keys.E) && !_pressedE)
        {
            _pressedE = true;
            Press(GameControl.Interact);
        }

        if (_pressedE && kstate.IsKeyUp(Keys.E))
            _pressedE = false;
    }
}