using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TestGame.UI;

public abstract class UIElement
{
    public readonly UIId Id;
    public bool Visible = true;
    public Rectangle Bounds;
    public Rectangle ScreenBounds;
    public HorizontalAlignment HorizontalAlignment = HorizontalAlignment.Left;
    public VerticalAlignment VerticalAlignment = VerticalAlignment.Top;

    public UIElement(UIId id)
    {
        Id = id;
    }
    public virtual void OnClick(Point clickPosition) { }
}