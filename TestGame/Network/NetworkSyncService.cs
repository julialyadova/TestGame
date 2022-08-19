using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using TestGame.Adapters;
using TestGame.Core.Entities.Creatures;
using TestGame.Core.Map;

namespace TestGame.Network;

public class NetworkSyncService
{
    private WorldMap _map;
    private Player _userPlayer;

    public NetworkSyncService(IServiceProvider services)
    {
        _map = services.GetRequiredService<WorldMap>();
        _userPlayer = services.GetRequiredService<PlayerController>().Player;
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
        Debug.WriteLine($"Sync : connected player spawned. Player id = {packet.Id}, name = {packet.Name}");
        _map.Players.Add(new Player()
        {
            Id = packet.Id,
            Name = packet.Name,
            TextureName = packet.Texture,
            Position = new Vector2(packet.X, packet.Y)
        });
    }
    
    public SpawnPlayerPacket GetPlayerData()
    {
        var packet = new SpawnPlayerPacket();
        packet.Id = _userPlayer.Id;
        packet.Name = _userPlayer.Name;
        packet.Texture = _userPlayer.TextureName;
        packet.X = _userPlayer.Position.X;
        packet.Y = _userPlayer.Position.Y;
        return packet;
    }
    
    public void SyncPlayer(SyncPlayerPacket packet)
    {
        var player = _map.Players.Find(p => p.Id == packet.Id);
        if (player == null)
            return;

        player.Position.X = packet.X;
        player.Position.Y = packet.Y;
    }
    
    public SyncPlayerPacket GetPlayerState()
    {
        var packet = new SyncPlayerPacket();
        packet.Id = _userPlayer.Id;
        packet.X = _userPlayer.Position.X;
        packet.Y = _userPlayer.Position.Y;
        return packet;
    }
}