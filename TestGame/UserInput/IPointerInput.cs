using Microsoft.Xna.Framework;

namespace TestGame.UserInput;

public interface IPointerInput : IUserInput
{
    public Point GetPosition();
    public MouseInputState GetState();
    public bool ClickTargetReached();
    public void EndClick();
}