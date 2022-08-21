using System;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using TestGame.Adapters;
using TestGame.Commands;
using TestGame.Core.Entities.Creatures;
using TestGame.Core.Map;
using TestGame.Network.Packets;

namespace TestGame.Network;

public class ClientPacketManager
{
    public bool PlayerConnected => _connected;
    private Config _config;
    private WorldMap _map;
    private Player _userPlayer;
    private LogCommand _logCommand;
    private SyncPlayerPacket _syncPacket;
    private bool _connected;
    
    public ClientPacketManager(IServiceProvider services)
    {
        _config = services.GetRequiredService<Config>();
        _map = services.GetRequiredService<WorldMap>();
        _userPlayer = services.GetRequiredService<PlayerController>().Player;
        _logCommand = services.GetRequiredService<LogCommand>();
    }

    public void UseUsername(string username)
    {
        _userPlayer.Name = username;
    }

    public JoinRequestPacket GetJoinRequestPacket()
    {
        return new JoinRequestPacket()
        {
            Username = _userPlayer.Name
        };
    }
    
    public JoinPacket GetJoinPacket()
    {
        return new JoinPacket()
        {
            Username = _userPlayer.Name,
            Texture = _userPlayer.TextureName
        };
    }

    public void OnJoinAccepted(JoinAcceptedPacket packet)
    {
        _logCommand.SetMessage("Join request was accepted! Loading map..").Execute();
        
        _userPlayer.Id = packet.PlayerId;
        Debug.WriteLine($"Client: join request accepted! Your Id is {packet.PlayerId}. Loading map..");
        
        _map.Load(packet.MapSeed);
        Debug.WriteLine($"Client: map loaded");
    }
    
    public void OnJoinRejected(JoinRejectedPacket packet)
    {
        _logCommand.SetMessage($"Join request was rejected : {packet.Reason}").Execute();
    }

    public void SpawnPlayer(SpawnPlayerPacket packet)
    {
        if (packet.PlayerId == _userPlayer.Id)
        {
            _userPlayer.Position = new Vector2(packet.X, packet.Y);
            _map.Players.Add(_userPlayer);
            _logCommand.SetMessage("Welcome to server!").Execute();
            _connected = true;

            _syncPacket = new SyncPlayerPacket()
            {
                PlayerId = _userPlayer.Id
            };
        }
        else if (!_map.Players.Exists(packet.PlayerId))
        {
            _map.Players.Add(new Player()
            {
                Id = packet.PlayerId,
                Name = packet.Name,
                TextureName = packet.Texture,
                Position = new Vector2(packet.X, packet.Y)
            });
        }
    }
    
    public SyncPlayerPacket GetSyncPlayerPacket()
    {
        _syncPacket.X = _userPlayer.Position.X;
        _syncPacket.Y = _userPlayer.Position.Y;
        _syncPacket.DirectionX = _userPlayer.Direction.X;
        _syncPacket.DirectionY = _userPlayer.Direction.Y;
        return _syncPacket;
    }

    public void SyncPlayer(SyncPlayerPacket packet)
    {
        if (packet.PlayerId != _userPlayer.Id && _map.Players.Exists(packet.PlayerId))
            _map.Players.FindById(packet.PlayerId).Position = new Vector2(packet.X, packet.Y);
    }

    public void Disconnect()
    {
        _connected = false;
        _map.UnLoad();
    }

    public void OnPlayerDiconnected(PlayerDisconnectedPacket packet)
    {
        if (packet.PlayerId == _userPlayer.Id)
        {
            Disconnect();
        }
        else
        {
            _map.Players.RemovePlayer(packet.PlayerId);
        }
    }
}