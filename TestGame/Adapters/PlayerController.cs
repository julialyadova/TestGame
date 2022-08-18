using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using TestGame.Core.Entities.Creatures;
using TestGame.Core.Map;

namespace TestGame.Adapters;

public class PlayerController
{
    public const int MoveSpeed = 5;
    
    public Player Player;
    private WorldMap _map;
    
    public PlayerController(IServiceProvider services)
    {
        _map = services.GetRequiredService<WorldMap>();
        Player = new Player();
        Player.Id = (byte)new Random().Next(0, 100);
        _map.Players.Add(Player);
    }

    public void Move(Vector2 direction)
    {
        var newPosition = Player.Position + direction * MoveSpeed;
        if (_map.TileIsEmpty(newPosition.ToPoint()))
            Player.Position = newPosition;    
    }
}