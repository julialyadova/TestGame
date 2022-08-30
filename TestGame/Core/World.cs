using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using TestGame.Core.Entities.Creatures;
using TestGame.Core.Entities.Structures;
using TestGame.Core.Map;
using TestGame.Core.Players;
using TestGame.Drawing;

namespace TestGame.Core;

public class World
{
    public bool IsLoaded;
    public readonly WorldMap Map;
    public readonly GamePlayers Players;
    public readonly PlayerController PlayerController;
    private readonly ILogger<World> _logger;

    public World(ILogger<World> logger)
    {
        _logger = logger;
        Map = new (new Point(1000,1000));
        Players = new();
        PlayerController = new PlayerController(Map);
        _logger.LogDebug("Initialized");
    }
    
    public void Click(Point position)
    {
        _logger.LogDebug("Clicked at {ClickPosition}", position);
        if (IsLoaded)
            Map.Build(new Wall(1), position);
    }

    public void SpawnMainPlayer(Player player)
    {
        PlayerController.Player = player;
        player.Position = Map.SpawnPoint.ToVector2();
        Players.Add(player);
        _logger.LogInformation("Player {Username} is set as main user-controlled player", player.Name);
    }

    public void SpawnPlayer(Player player)
    {
        player.Position = Map.SpawnPoint.ToVector2();
        Players.Add(player);
        _logger.LogInformation("Player {Username} spawned", player.Name);
    }

    public Player GetMainPlayer()
    {
        return PlayerController.Player;
    }

    public void Quit()
    {
        IsLoaded = false;
    }
}