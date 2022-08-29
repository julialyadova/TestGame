using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework.Input;
using Myra.Graphics2D.UI;
using TestGame.Core;
using TestGame.Core.Entities.Creatures;
using TestGame.Network;

namespace TestGame.States;

public class JoinGameState : LoadGameState
{
    private NetworkServiceProvider _network;
    private ILogger<JoinGameState> _logger;
    private World _world;
    private Config _config;

    public JoinGameState(IServiceProvider services) : base(services)
    {
        _network = services.GetRequiredService<NetworkServiceProvider>();
        _world = services.GetRequiredService<World>();
        _logger = services.GetRequiredService<ILogger<JoinGameState>>();
        _config = services.GetRequiredService<Config>();
    }

    public override void Enter()
    {
        _logger.LogInformation("Joining the game");
        _network.Join(
            _config.ServerHost,
            _config.ServerPort,
            _config.ConnectionKey,
            _config.Username,
            OnJoinResult);
        _world.IsLoaded = false;
    }

    private void OnJoinResult(JoinResult result)
    {
        if (result == JoinResult.Rejected)
            SetState(MainMenuState);
    }
    

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
        
        _network.Update();
        
        if (_world.IsLoaded)
        {
            _logger.LogInformation("Map loaded");
            var player = new Player();
            player.Name = "Client";
            _world.SpawnMainPlayer(player);
            SetState(ExploreWorldState);
        }
    }
}