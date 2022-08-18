using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TestGame.UserInput;

public class WASDMoveInput : MoveInput
{
    private Vector2 _direction;
    
    public Vector2 GetDirection()
    {
        return _direction;
    }
    
    public override void Update(GameTime gameTime)
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
        
        Move(_direction, gameTime);
    }
}