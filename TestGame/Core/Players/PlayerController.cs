using System;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using TestGame.Core.Entities.Creatures;
using TestGame.Core.Map;
using TestGame.UserInput;

namespace TestGame.Adapters;

public class PlayerController
{
    private const int MoveSpeed = 5;
    
    public Player Player;
    public Point Focus;
    private Config _config;
    private WorldMap _map;
    
    public PlayerController(IServiceProvider services)
    {
        _config = services.GetRequiredService<Config>();
        _map = services.GetRequiredService<WorldMap>();
        
        Player = new Player();
        Player.Name = _config.Username;
        Player.TextureName = _config.PlayerTexture;
    }

    public void Move(Vector2 direction, float deltaTime)
    {
        if (direction == Vector2.Zero)
            return;
        
        var newPosition = Player.Position + direction * MoveSpeed * deltaTime;
        var tile = newPosition.ToPoint();
        if (_map.CanWalkTrough(tile))
            Player.Position = newPosition;

        Player.Direction = direction;
        Focus = Player.Position.ToPoint() + Player.Direction.ToPoint();
    }

    public void Interact()
    {
        Debug.WriteLine("Interact");
        var target = _map.GetStructureAt(Focus);
        if (target != null)
            _map.Remove(target);
    }
}