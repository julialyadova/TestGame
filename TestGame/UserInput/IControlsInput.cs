using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace TestGame.UserInput;

public interface IControlsInput : IUserInput
{
    public bool ControlPressed();
    public GameControl GetPressedControl();
}

public enum GameControl
{
    None,
    Interact,
    Attack
}