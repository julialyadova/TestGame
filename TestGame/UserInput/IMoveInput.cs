using Microsoft.Xna.Framework;

namespace TestGame.UserInput;

public interface IMoveInput: IUserInput
{
    public bool IsMoving();
    public Vector2 GetDirection();
}