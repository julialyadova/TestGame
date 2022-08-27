using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TestGame.UserInput;

public class WASDMoveInput : IMoveInput
{
    private Vector2 _direction;

    public bool IsMoving()
    {
        return _direction.X != 0 || _direction.Y != 0;
    }

    public Vector2 GetDirection()
    {
        return _direction;
    }
    
    public void UpdateState()
    {
        _direction = Vector2.Zero;
        
        var kstate = Keyboard.GetState();

        if (kstate.IsKeyDown(Keys.W))
        {
            _direction.Y -= 1;
        }

        if (kstate.IsKeyDown(Keys.S))
        {
            _direction.Y += 1;
        }

        if (kstate.IsKeyDown(Keys.D))
        {
            _direction.X += 1;
        }
        
        if (kstate.IsKeyDown(Keys.A))
        {
            _direction.X -= 1;
        }

        if (_direction.X != 0 && _direction.Y != 0)
        {
            _direction.Normalize();
        }
    }
}