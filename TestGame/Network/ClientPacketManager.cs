using System;
using System.Diagnostics;
using LiteNetLib.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using TestGame.Adapters;
using TestGame.Commands;
using TestGame.Core;
using TestGame.Core.Entities.Base;
using TestGame.Core.Entities.Creatures;
using TestGame.Core.Map;
using TestGame.Network.Packets;

namespace TestGame.Network;

public class ClientPacketManager
{
    public bool PlayerConnected => _connected;
    public Action<INetSerializable> OnSyncRequired;
    
    private World _world;
    private Player _userPlayer;
    private LogCommand _logCommand;
    private SyncPlayerPacket _syncPacket;
    private bool _connected;
    
    public ClientPacketManager(IServiceProvider services)
    {
        _world = services.GetRequiredService<World>();
        _userPlayer = services.GetRequiredService<PlayerController>().Player;
        _logCommand = services.GetRequiredService<LogCommand>();
        
        _world.Map.OnStructureRemoved += structure => RequireSync(GetStructureRemovedPacket(structure));
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
        
        _world.Load(new Save() {MapSeed = packet.MapSeed});
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
            _world.Players.Add(_userPlayer);
            _logCommand.SetMessage("Welcome to server!").Execute();
            _connected = true;

            _syncPacket = new SyncPlayerPacket()
            {
                PlayerId = _userPlayer.Id
            };
        }
        else if (!_world.Players.Exists(packet.PlayerId))
        {
            _world.Players.Add(new Player()
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
        if (packet.PlayerId != _userPlayer.Id && _world.Players.Exists(packet.PlayerId))
            _world.Players.FindById(packet.PlayerId).Position = new Vector2(packet.X, packet.Y);
    }

    public void Disconnect()
    {
        _connected = false;
        _world.Quit();
    }

    public void OnPlayerDiconnected(PlayerDisconnectedPacket packet)
    {
        if (packet.PlayerId == _userPlayer.Id)
        {
            Disconnect();
        }
        else
        {
            _world.Players.RemovePlayer(packet.PlayerId);
        }
    }

    public void OnStructureRemoved(StructureRemovedPacket packet)
    {
        _world.Map.RemoveStructure(packet.X, packet.Y);
    }
    
    private void RequireSync(INetSerializable packet)
    {
        if (_connected && OnSyncRequired != null)
            OnSyncRequired(packet);
    }


    private StructureRemovedPacket GetStructureRemovedPacket(Structure structure)
    {
        return new StructureRemovedPacket()
        {
            X = structure.Position.X,
            Y = structure.Position.Y
        };
    }
}