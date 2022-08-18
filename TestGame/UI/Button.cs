using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TestGame.Commands;

namespace TestGame.UI;

public class Button : UIElement
{
    private ICommand _command;

    public Button(Rectangle bounds, string texture = null) : base(bounds, texture)
    {
    }
    
    public void SetCommand(ICommand command)
    {
        _command = command;
    }

    public override void OnClick(Point clickPosition)
    {
        _command.Execute();
    }
}