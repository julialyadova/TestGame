using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TestGame.Input
{
    public class Pointer
    {
        public Point Position { get; private set; }
        public PointerAction Action { get; private set; }

        public void UpdateState(MouseState mouseState)
        {
            Position = mouseState.Position;

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                if (Action == PointerAction.Hover || Action == PointerAction.Release)
                    Action = PointerAction.Click;
                else if (Action == PointerAction.Click)
                    Action = PointerAction.Hold;
            }
            else
            {
                if (Action == PointerAction.Click || Action == PointerAction.Hold)
                    Action = PointerAction.Release;
                else
                    Action = PointerAction.Hover;
            }
        }
    }

    public enum PointerAction
    {
        Hover,
        Click,
        Hold,
        Release
    }
}
