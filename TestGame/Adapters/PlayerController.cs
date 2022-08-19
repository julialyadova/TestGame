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
    private Config _config;
    
    public PlayerController(IServiceProvider services)
    {
        _map = services.GetRequiredService<WorldMap>();
        _config = services.GetRequiredService<Config>();
        
        Player = new Player();
        Player.Id = _config.PlayerId == 0 ? (byte)new Random().Next(0, 100) : (byte)_config.PlayerId;
        Player.Name = _config.PlayerName;
        Player.TextureName = _config.PlayerTexture;
        _map.Players.Add(Player);
    }

    public void Move(Vector2 direction)
    {
        var newPosition = Player.Position + direction * MoveSpeed;
        if (_map.TileIsEmpty(newPosition.ToPoint()))
            Player.Position = newPosition;    
    }
}