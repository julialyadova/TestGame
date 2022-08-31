using Microsoft.Xna.Framework;

namespace TestGame.UserInput;

public interface IPointerInput : IUserInput
{
    public Point GetPosition();
    public bool IsClick();
}