using Microsoft.Xna.Framework.Input;
using TestGame.Core.Entities.Creatures;
using TestGame.Core.Tools;
using TestGame.Tools;

namespace TestGame.Commands;

public class SelectBuildingToolCommand: ICommand
{
    private Player _player;
    public SelectBuildingToolCommand(Player tool)
    {
        _player = tool;
    }

    public void Execute()
    {
        _player.SelectedTool = new BuildingTool();
        Mouse.SetCursor(MouseCursor.Hand);
    }
}