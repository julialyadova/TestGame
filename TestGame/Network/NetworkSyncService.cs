using System;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using TestGame.Core.Map;

namespace TestGame.Network;

public class NetworkSyncService
{
    private WorldMap _map;

    public NetworkSyncService(IServiceProvider services)
    {
        _map = services.GetRequiredService<WorldMap>();
    }

    public int GetId()
    {
        return 0;
    }

    public int GetMapSeed()
    {
        return _map.Seed;
    }
    
    public void Join(JoinPacket packet)
    {
        Debug.WriteLine($"Sync : New Player is joining. Player id = {packet.Id}");
    }
    
    public void AcceptJoin(JoinAcceptPacket packet)
    {
        new MapGenerator().Generate(_map, packet.MapSeed);
        Debug.WriteLine($"Sync : Server accepted connection. Server player id = {packet.Id}, seed = {packet.MapSeed}");
    }

    public void SpawnPlayer(SpawnPlayerPacket packet)
    {
        
    }
}