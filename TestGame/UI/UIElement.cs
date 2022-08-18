using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TestGame.UI;

public abstract class UIElement
{
    public readonly Rectangle Bounds;
    public Texture2D Texture;
    public readonly string TextureName;

    public UIElement(Rectangle bounds, string texture = null)
    {
        Bounds = bounds;
        TextureName = texture;
    }

    public abstract void OnClick(Point clickPosition);
}