using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using LiteNetLib;
using LiteNetLib.Utils;
using Microsoft.Extensions.DependencyInjection;
using TestGame.Core;
using TestGame.Core.Map;
using TestGame.Network.Packets;

namespace TestGame.Network;

public class ServerPacketManager
{
    private Dictionary<string, byte> _registeredPlayers; //todo: store username-id dictionary on host (players' saves)
    private Dictionary<int, byte> _connectedPeers;
    private byte _lastPlayerId; //loads from stored username-id file
    private World _world;

    public ServerPacketManager(IServiceProvider services)
    {
        _world = services.GetRequiredService<World>();
        _registeredPlayers = new();
        _connectedPeers = new();
    }

    public INetSerializable GetResultOfJoinRequestPacket(NetPeer peer, JoinRequestPacket packet)
    {
        Debug.WriteLine($"Server : received join request from {peer.EndPoint} - Player {packet.Username} ");
        if (_connectedPeers.ContainsKey(peer.Id))
        {
            Debug.WriteLine($"Server : join request from {peer.EndPoint} was rejected. Player with id {_connectedPeers[peer.Id]} is using this endpoint ");
            return new JoinRejectedPacket("Other Player is using this endpoint");
        }
        
        if (_registeredPlayers.ContainsKey(packet.Username) && _connectedPeers.ContainsValue(_registeredPlayers[packet.Username]))
        {
            Debug.WriteLine($"Server : join request from {peer.EndPoint} was rejected. Player with id {_connectedPeers[peer.Id]} is connected");
            return new JoinRejectedPacket($"Player {packet.Username} is already connected");
        }
        
        Debug.WriteLine($"Server : join request for {packet.Username} was accepted");

        byte playerId;
        if (!_registeredPlayers.ContainsKey(packet.Username))
        {
            playerId = _lastPlayerId++;
            Debug.WriteLine($"Server : {packet.Username} joined the server the first time! {packet.Username}'s new id is {playerId}");
            _registeredPlayers[packet.Username] = playerId;
            _connectedPeers[peer.Id] = playerId;
        }
        else
        {
            playerId = _registeredPlayers[packet.Username];
            Debug.WriteLine($"Server : welcome back player {packet.Username}! {packet.Username}'s id is {playerId}");
            _connectedPeers[peer.Id] = playerId;
        }
        return new JoinAcceptedPacket()
        {
            PlayerId = playerId,
            MapSeed = _world.Map.Seed
        };
    }

    public bool PeerIsAccepted(NetPeer peer)
    {
        return _connectedPeers.ContainsKey(peer.Id);
    }
    
    public SpawnPlayerPacket GetSpawnPacket(NetPeer peer, JoinPacket packet)
    {
        return new SpawnPlayerPacket()
        {
            PlayerId = _connectedPeers[peer.Id],
            Name = packet.Username,
            Texture = packet.Texture,
            X = _world.SpawnPoint.X,
            Y = _world.SpawnPoint.Y
        };
    }

    public IEnumerable<SpawnPlayerPacket> GetSpawnPacketsOfConnectedPlayers()
    {
        return _world.Players.Select(player  => new SpawnPlayerPacket()
        {
            PlayerId = player.Id,
            Name = player.Name,
            Texture = player.TextureName,
            X = player.Position.X,
            Y = player.Position.Y
        });
    }

    public PlayerDisconnectedPacket GetPlayerDisconnectedPacket(NetPeer peer)
    {
        return new PlayerDisconnectedPacket()
        {
            PlayerId = _connectedPeers[peer.Id]
        };
    }


}