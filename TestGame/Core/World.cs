using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using System;
using TestGame.Core.Entities.Base;
using TestGame.Core.Entities.Creatures;
using TestGame.Core.Entities.Structures;
using TestGame.Core.Map;
using TestGame.Core.Players;
using TestGame.Drawing;

namespace TestGame.Core;

public class World
{
    private readonly ILogger<World> _logger;

    public Action OnExit;
    public readonly WorldMap Map;
    public Point SpawnPoint = new(10,10);
    public readonly GamePlayers Players;
    public readonly PlayerController CurrentPlayer;
    public readonly Camera MainCamera;

    public World(ILogger<World> logger)
    {
        _logger = logger;
        
        Map = new (new Point(1000,1000));
        Players = new();
        CurrentPlayer = new PlayerController(Map);
        MainCamera = new Camera();
        _logger.LogDebug("Initialized");
    }
    
    public void Click(Point position)
    {
        _logger.LogDebug("Clicked at {ClickPosition}", position);
        Map.Build(new Tree(), ScreenAdapter.GetMapPosition(MainCamera.GetWorldPosition(position)).ToPoint());
    }

    public void Animate()
    {
        foreach (var entity in Map.GetEntities())
        {
            if (entity is IAnimated animatedEntity)
                animatedEntity.Animate();
        }
    }

    public void SpawnMainPlayer(Player player)
    {
        CurrentPlayer.Player = player;
        player.Position = SpawnPoint.ToVector2();
        Players.Add(player);
        _logger.LogInformation("Player {Username} is set as main user-controlled player", player.Name);
    }

    public void SpawnPlayer(Player player)
    {
        player.Position = SpawnPoint.ToVector2();
        Players.Add(player);
        _logger.LogInformation("Player {Username} spawned", player.Name);
    }

    public void Exit()
    {
        OnExit?.Invoke();
    }
}