using System;
using LiteNetLib;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TestGame.Core.Entities.Creatures;

namespace TestGame.Network;

public class NetworkServiceProvider : ISyncPacketListener
{
    private INetworkService _networkService;
    private IServiceProvider _services;
    private ILogger<NetworkServiceProvider> _logger;
    private WorldSyncService _worldSyncService;

    public NetworkServiceProvider(IServiceProvider services)
    {
        _services = services;
        _logger = services.GetRequiredService<ILogger<NetworkServiceProvider>>();
    }

    public void Host()
    {
        _logger.LogInformation("Hosting game");
       var server = new Server(_services, _worldSyncService);
       server.Start();
       _networkService = server;
    }
    
    public void Join(string host, int port, string key, string username, Action<JoinResult> callback)
    {
        _logger.LogInformation("Joining {Address}:{Port} as {Username}", host, port, username);
        var client = new Client(_services, _worldSyncService);
        client.Connect(host, port, key, username, callback);
        _networkService = client;
    }

    public void Stop()
    {
        _networkService.Stop();
        _logger.LogInformation("Network service stopped");
    }

    public void Update()
    {
        _networkService.Update();
    }

    public void SyncPlayer(Player player)
    {
        _networkService.SendSyncPacket(new SyncPlayerPacket()
        {
            X = player.Position.X,
            Y = player.Position.Y,
            DirectionX = player.Direction.X,
            DirectionY = player.Direction.Y
        }, DeliveryMethod.Unreliable);
    }

    public void OnSyncPlayerPacketReceived(SyncPlayerPacket packet)
    {
        
    }
}