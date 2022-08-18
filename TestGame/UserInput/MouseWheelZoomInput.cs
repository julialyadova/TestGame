using Microsoft.Xna.Framework.Input;

namespace TestGame.UserInput;

public class MouseWheelZoomInput : IZoomInput
{
    private int previousScrollWheelValue = 0;
    private int deltaScrollWheelValue = 0;
    
    public void UpdateState()
    {
        var value = Mouse.GetState().ScrollWheelValue;
        deltaScrollWheelValue = value - previousScrollWheelValue;
        previousScrollWheelValue = value;
    }

    public int GetZoomChangeValue()
    {
        return deltaScrollWheelValue;
    }
}