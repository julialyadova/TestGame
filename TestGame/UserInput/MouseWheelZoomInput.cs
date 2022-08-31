using Microsoft.Xna.Framework.Input;

namespace TestGame.UserInput;

public class MouseWheelZoomInput : IZoomInput
{
    private const float ZoomSpeed = 0.0005f;
    
    private int _previousScrollWheelValue;
    private int _deltaScrollWheelValue;
    
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

    public float GetZoomValue()
    {
        return _deltaScrollWheelValue * ZoomSpeed;
    }
}