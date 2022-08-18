using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TestGame.UserInput;

public abstract class PointerInput : IUserInput
{
    public Point Position => GetPosition();
    public MouseInputState State { get; protected set; }
    private List<Func<Point, bool>> _listeners;

    public void AddOnClickListener(Func<Point, bool> listener)
    {
        if (_listeners == null)
            _listeners = new List<Func<Point, bool>>();
        _listeners.Add(listener);
    }
    
    protected void Click()
    {
        Console.WriteLine("click");
        foreach (var listener in _listeners)
        {
            if (listener(Position))
                break;
        }
    }

    protected abstract Point GetPosition();
    public abstract void Update(GameTime gameTime);
}