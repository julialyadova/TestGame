using System;
using Microsoft.Xna.Framework;

namespace TestGame.UserInput;

public abstract class MoveInput: IUserInput
{
    private Action<Vector2, GameTime> _listener;
    public void AddOnMoveListener(Action<Vector2, GameTime> listener)
    {
        _listener += listener;
    }

    protected void Move(Vector2 direction, GameTime gameTime)
    {
        _listener?.Invoke(direction,gameTime);
    }
    public abstract void Update(GameTime gameTime);
}