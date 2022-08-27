using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TestGame.UserInput;

public class MousePointerInput : IPointerInput
{
    private MouseInputState _state;
    public Point GetPosition()
    {
        return Mouse.GetState().Position;
    }

    public bool IsClick()
    {
        return _state == MouseInputState.Click;
    }

    public void UpdateState()
    {
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