using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace TestGame.UserInput;

public abstract class ZoomInput: IUserInput
{
    private Action<int> _listener;
    public void AddOnZoomListener(Action<int> listener)
    {
        _listener += listener;
    }

    protected void Zoom(int value)
    {
        _listener?.Invoke(value);
    }
    public abstract void Update(GameTime gameTime);
}