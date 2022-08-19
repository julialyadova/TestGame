using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TestGame.UI;

public class TextBox : UIElement
{
    public int TextLength { get; protected set; }
    public string Text { get; protected set; } = "";
    public Color Color = Color.Black;
    public HorizontalAlignment TextAlignment = HorizontalAlignment.Left;

    public TextBox(UIId id) : base(id) { }

    public void SetText(string text)
    {
        Text = text ?? "";
        TextLength = Text.Length;
    }
}