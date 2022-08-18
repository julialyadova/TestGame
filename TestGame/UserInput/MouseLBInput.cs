using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TestGame.UserInput;

public class MouseLBInput : IPointerInput
{
    private MouseInputState _state;
    private bool _clickTargetReached;

    public MouseInputState GetState()
    {
        return _state;
    }

    public Point GetPosition()
    {
        return Mouse.GetState().Position;
    }

    public bool ClickTargetReached()
    {
        return _clickTargetReached;
    }

    public void EndClick()
    {
        _clickTargetReached = true;
    }

    public void UpdateState()
    {
        _clickTargetReached = false;
        if (Mouse.GetState().LeftButton == ButtonState.Pressed)
        {
            if (_state == MouseInputState.Hover || _state == MouseInputState.Release)
            {
                _state = MouseInputState.Click;
            }
            else if (_state == MouseInputState.Click)
            {
                _state = MouseInputState.Hold;
            }
        }
        else
        {
            if (_state != MouseInputState.Release)
            {
                _state = MouseInputState.Release;
            }
            else
            {
                _state = MouseInputState.Hover;
            }
        }
    }
}