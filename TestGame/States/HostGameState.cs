using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TestGame.Core;
using TestGame.Core.Entities.Creatures;
using TestGame.Core.Map;
using TestGame.Network;

namespace TestGame.States;

public class HostGameState : LoadGameState
{
    private NetworkServiceProvider _network;
    private ILogger<HostGameState> _logger;
    private World _world;
    private MapGenerator _mapGenerator;

    public HostGameState(IServiceProvider services) : base(services)
    {
        _network = services.GetRequiredService<NetworkServiceProvider>();
        _world = services.GetRequiredService<World>();
        _logger = services.GetRequiredService<ILogger<HostGameState>>();
        _mapGenerator = new MapGenerator();
    }

    public override void Enter()
    {
        _logger.LogInformation($"Generating map");
        Task.Run(() => _mapGenerator.Generate(_world.Map));
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);

        ShowLoadingMessage($"{100 * _mapGenerator.Progress:0.00}%");
        
        if (_mapGenerator.Done)
        {
            _logger.LogInformation("Map generated. Starting Server");
            var player = new Player();
            player.Name = "Host";
            _world.SpawnMainPlayer(player);
            _network.Host();
            SetState(ExploreWorldState);
        }
    }
}