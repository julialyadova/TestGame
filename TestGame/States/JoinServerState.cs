using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TestGame.Core;
using TestGame.Core.Entities.Creatures;
using TestGame.Network;
using TestGame.States.Base;
using TestGame.UI.Abstractions;

namespace TestGame.States;

public class JoinServerState : MainState
{
    private readonly NetworkServiceProvider _network;
    private readonly ILogger<JoinServerState> _logger;
    private readonly World _world;
    private readonly Config _config;
    private readonly ILoadingUI _loadingUI;
    private readonly bool _worldLoaded = false;

    public JoinServerState(IServiceProvider services)
    {
        _network = services.GetRequiredService<NetworkServiceProvider>();
        _world = services.GetRequiredService<World>();
        _logger = services.GetRequiredService<ILogger<JoinServerState>>();
        _config = services.GetRequiredService<Config>();
        _loadingUI = services.GetRequiredService<ILoadingUI>();
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
    }

    private void OnJoinResult(JoinResult result)
    {
        if (result == JoinResult.Rejected)
        {
            SetState(MainMenuState);
        }
        else if (result == JoinResult.Accepted)
        {
            //todo: send request to get world data
        }
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
        
        _network.Update();
        _loadingUI.Update(deltaTime);
        
        if (_worldLoaded)
        {
            _logger.LogInformation("Map loaded");
            var player = new Player();
            player.Name = "Client";
            _world.SpawnMainPlayer(player);
            SetState(PlayState);
        }
    }

    public override void Draw()
    {
        base.Draw();
        _loadingUI.Draw();
    }
}