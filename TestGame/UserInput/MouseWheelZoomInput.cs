using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TestGame.UserInput;

public class MouseWheelZoomInput : IZoomInput
{
    private int _previousScrollWheelValue = 0;
    private int _deltaScrollWheelValue = 0;
    
    public void UpdateState()
    {
        var value = Mouse.GetState().ScrollWheelValue;
        _deltaScrollWheelValue = value - _previousScrollWheelValue;
        _previousScrollWheelValue = value;
    }

    public bool IsZooming()
    {
        return _deltaScrollWheelValue != 0;
    }

    public int GetZoomValue()
    {
        return _deltaScrollWheelValue;
    }
}