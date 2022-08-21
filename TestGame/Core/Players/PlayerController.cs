using System;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using TestGame.Core.Entities.Creatures;
using TestGame.Core.Map;

namespace TestGame.Adapters;

public class PlayerController
{
    public const int MoveSpeed = 5;
    
    public Player Player;
    private Config _config;
    
    public PlayerController(IServiceProvider services)
    {
        _config = services.GetRequiredService<Config>();
        
        Player = new Player();
        Player.Name = _config.Username;
        Player.TextureName = _config.PlayerTexture;
    }

    public void Move(Vector2 direction)
    {
        Player.Direction = direction;
    }
}