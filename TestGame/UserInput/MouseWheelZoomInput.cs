using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TestGame.UserInput;

public class MouseWheelZoomInput : ZoomInput
{
    private int _previousScrollWheelValue = 0;
    private int _deltaScrollWheelValue = 0;
    
    public override void Update(GameTime gameTime)
    {
        var value = Mouse.GetState().ScrollWheelValue;
        _deltaScrollWheelValue = value - _previousScrollWheelValue;
        _previousScrollWheelValue = value;
        if (_deltaScrollWheelValue != 0)
            Zoom(_deltaScrollWheelValue * 2);
    }
}