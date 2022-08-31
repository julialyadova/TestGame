using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TestGame.Core;
using TestGame.Core.Entities.Creatures;
using TestGame.Core.Map;
using TestGame.Network;
using TestGame.States.Base;
using TestGame.UI.Abstractions;

namespace TestGame.States;

public class StartServerState : MainState
{
    private NetworkServiceProvider _network;
    private ILogger<StartServerState> _logger;
    private World _world;
    private MapGenerator _mapGenerator;
    private ILoadingUI _loadingUI;

    public StartServerState(IServiceProvider services)
    {
        _network = services.GetRequiredService<NetworkServiceProvider>();
        _world = services.GetRequiredService<World>();
        _logger = services.GetRequiredService<ILogger<StartServerState>>();
        _mapGenerator = new MapGenerator();
        _loadingUI = services.GetRequiredService<ILoadingUI>();
    }

    public override void Enter()
    {
        _logger.LogInformation($"Generating map");
        Task.Run(() => _mapGenerator.Generate(_world.Map));
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);

        _loadingUI.ShowMessage($"{100 * _mapGenerator.Progress:0.00}%");
        _loadingUI.Update(deltaTime);

        if (_mapGenerator.Done)
        {
            _logger.LogInformation("Map generated. Starting Server");
            var player = new Player();
            player.Name = "Host";
            _world.SpawnMainPlayer(player);
            _network.Host();
            SetState(PlayState);
        }
    }

    public override void Draw()
    {
        base.Draw();
        _loadingUI.Draw();
    }
}