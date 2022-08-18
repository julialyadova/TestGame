using Microsoft.Xna.Framework;
using TestGame.Core.Entities.Base;
using TestGame.Entities;
using TestGame.Extensions;
using TestGame.Tools;

namespace TestGame.Core.Entities.Creatures;

public class Player : Entity
{
    private const int MoveSpeed = 5;
    
    public Vector2 Position;
    public Vector2 Size = new Vector2(0.8f, 1.8f);
    
    public Tool SelectedTool {
        get => _selectedTool; 
        set => _selectedTool = value;
    }
    
    private Tool _selectedTool = new SelectorTool();
    private WorldMap _map;

    public Player(WorldMap map)
    {
        _map = map;
    }

    public void Move(Vector2 direction)
    {
        if (_map.TileIsEmpty((Position + direction * MoveSpeed).ToPoint()))
            Position += direction * MoveSpeed;
    }
}