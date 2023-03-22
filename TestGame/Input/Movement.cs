using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TestGame.Input
{
    public class Movement
    {
        private const Keys MoveUpKey = Keys.W;
        private const Keys MoveDownKey = Keys.S;
        private const Keys MoveLeftKey = Keys.A;
        private const Keys MoveRightKey = Keys.D;
        private const Keys MoveFasterKey = Keys.LeftShift;
        private Vector2 _direction;

        public Vector2 Direction => _direction;
        public MovementState State;

        public void UpdateState(KeyboardState keyboardState)
        {
            _direction = Vector2.Zero;

            if (keyboardState.IsKeyDown(MoveUpKey))
                _direction.Y -= 1;

            if (keyboardState.IsKeyDown(MoveDownKey))
                _direction.Y += 1;

            if (keyboardState.IsKeyDown(MoveLeftKey))
                _direction.X -= 1;

            if (keyboardState.IsKeyDown(MoveRightKey))
                _direction.X += 1;           

            if (_direction.X != 0 && _direction.Y != 0)
                _direction.Normalize();

            if (_direction.X == 0 && _direction.Y == 0)
                State = MovementState.NotMoving;
            else if (keyboardState.IsKeyDown(MoveFasterKey))
                State = MovementState.Running;
            else
                State = MovementState.Walking;
        }
    }

    public enum MovementState
    {
        NotMoving,
        Walking,
        Running
    }
}
