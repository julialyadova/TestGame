using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TestGame.UserInput;

public class MousePointerInput : PointerInput
{
    protected override Point GetPosition()
    {
        return Mouse.GetState().Position;
    }

    public override void Update(GameTime gameTime)
    {
        if (Mouse.GetState().LeftButton == ButtonState.Pressed)
        {
            if (State == MouseInputState.Hover || State == MouseInputState.Release)
            {
                State = MouseInputState.Click;
                Click();
            }
            else if (State == MouseInputState.Click)
            {
                State = MouseInputState.Hold;
            }
        }
        else
        {
            if (State != MouseInputState.Release)
            {
                State = MouseInputState.Release;
            }
            else
            {
                State = MouseInputState.Hover;
            }
        }
    }
}