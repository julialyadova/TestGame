using Microsoft.Xna.Framework;

namespace TestGame.UserInput;

public interface IMoveInput: IUserInput
{
    Vector2 GetDirection();
}