using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace TestGame.UserInput;

public interface IZoomInput: IUserInput
{
    public bool IsZooming();
    public int GetZoomValue();
}