using System;
using Microsoft.Xna.Framework;
using TestGame.Commands;

namespace TestGame.UI;

public class Button : UIElement
{
    public string Backgroundtexture;
    public ICommand Command;
    public Action Click;
    public int TextLength { get; protected set; }
    public string Text { get; protected set; } = "button";
    public Color TextColor = Color.Black;
    
    public void SetText(string text)
    {
        Text = text ?? "";
        TextLength = Text.Length;
    }

    public override void OnClick(Point clickPosition)
    {
        Command?.Execute();
        Click?.Invoke();
    }

    public Button(UIId id) : base(id) { }
}