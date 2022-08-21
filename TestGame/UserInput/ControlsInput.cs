using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace TestGame.UserInput;

public abstract class ControlsInput : IUserInput
{
    private Action<GameControl> _listeners;

    public void AddOnControlPressedListener(Action<GameControl> listener)
    {
        _listeners += listener;
    }
    
    protected void Press(GameControl control)
    {
        Console.WriteLine("Control key pressed: " + control);
        _listeners?.Invoke(control);
    }
    public abstract void Update(GameTime gameTime);
}

public enum GameControl
{
    Interact,
    Attack
}