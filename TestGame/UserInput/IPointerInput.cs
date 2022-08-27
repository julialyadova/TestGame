using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TestGame.UserInput;

public interface IPointerInput : IUserInput
{
    public Point GetPosition();
    public bool IsClick();
}