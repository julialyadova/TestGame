using System;
using System.Diagnostics;
using LiteNetLib;
using LiteNetLib.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TestGame.Core.Entities.Creatures;
using TestGame.Network.Packets;

namespace TestGame.Network;

public class Server : INetworkService
{
    private NetManager _server;
    private NetDataWriter _writer;
    private ServerPacketManager _packetManager;
    private Config _config;
    private int _port;
    private ILogger<Server> _logger;
    private ISyncPacketListener _syncPacketListener;

    public Server(IServiceProvider services, ISyncPacketListener syncPacketListener)
    {
        _syncPacketListener = syncPacketListener;
        EventBasedNetListener listener = new EventBasedNetListener();
        _server = new NetManager(listener);
        _writer = new NetDataWriter();

        _packetManager = services.GetRequiredService<ServerPacketManager>();
        _config = services.GetRequiredService<Config>();
        _port = _config.ServerPort;
        _logger = services.GetRequiredService<ILogger<Server>>();

        listener.ConnectionRequestEvent += OnConnectionRequestEvent;
        listener.PeerConnectedEvent += OnPeerConnectedEvent;
        listener.PeerDisconnectedEvent += OnPeerDisconnectedEvent;
        listener.NetworkReceiveEvent += OnNetworkReceiveEvent;
    }

    public void SetPort(int port)
    {
        _port = port;
        _logger.LogDebug("Server port is set to {Port}", port);
    }
    
    public void Start()
    {
        _server.Start(_port);
        _logger.LogInformation("Server started on port {Port}", _port);
    }

    public void Update()
    {
        _server.PollEvents();
    }

    public void SendSyncPacket(INetSerializable packet, DeliveryMethod deliveryMethod)
    {
        _writer.Reset();
        _writer.Put(packet);
        _server.SendToAll(_writer, deliveryMethod);
    }

    public void Stop()
    {
        _server.Stop();
        _logger.LogInformation("Server stopped");
    }

    private void OnConnectionRequestEvent(ConnectionRequest request)
    {
        _logger.LogInformation("Received connection request form {Address}", request.RemoteEndPoint);
        if (_server.ConnectedPeersCount < _config.MaxConnections)
        {
            request.AcceptIfKey(_config.ConnectionKey);
        }
        else
        {
            request.Reject();
            _logger.LogInformation("Connection form {Address} rejected - too many connections.", request.RemoteEndPoint);
        }
    }

    private void OnPeerConnectedEvent(NetPeer peer)
    {
        _logger.LogInformation("Peer {PeerId} {Address} connected",peer.Id, peer.EndPoint);
    }
    
    private void OnPeerDisconnectedEvent(NetPeer peer, DisconnectInfo disconnectinfo)
    {
        _logger.LogInformation("Peer {PeerId} {Address} disconnected",peer.Id, peer.EndPoint);
        
        //send all players PlayerDisconnected packet
        _writer.Reset();
        _writer.Put(_packetManager.GetPlayerDisconnectedPacket(peer));
        _server.SendToAll(_writer, DeliveryMethod.ReliableUnordered);
    }

    private void OnNetworkReceiveEvent(NetPeer peer, NetPacketReader reader, byte channel, DeliveryMethod deliveryMethod)
    {
        var packetType = (PacketType) reader.PeekByte();
        switch (packetType)
        {
            case PacketType.RequestJoin:
                OnRequestJoinPacketReceived(peer, reader.Get<JoinRequestPacket>());
                break;
            case PacketType.Join:
                OnJoinPacketReceived(peer, reader.Get<JoinPacket>());
                break;
            case PacketType.SyncPlayer:
                OnSyncPacketReceived(peer,reader.Get<SyncPlayerPacket>(), DeliveryMethod.Unreliable);
                break;
            case PacketType.StructureRemoved:
                OnSyncPacketReceived(peer, reader.Get<StructureRemovedPacket>(), DeliveryMethod.ReliableUnordered);
                break;
        }
        reader.Recycle();
    }

    private void OnRequestJoinPacketReceived(NetPeer peer, JoinRequestPacket packet)
    {
        _writer.Reset();
        _writer.Put(_packetManager.GetResultOfJoinRequestPacket(peer, packet));
        peer.Send(_writer, DeliveryMethod.ReliableOrdered);
        
        if (!_packetManager.PeerIsAccepted(peer))
            peer.Disconnect();
    }
    
    private void OnJoinPacketReceived(NetPeer peer, JoinPacket packet)
    {
        if (!_packetManager.PeerIsAccepted(peer))
            return;
        
        //send new peer PlayerSpawn packets of all connected players
        foreach (var spawnPacket in _packetManager.GetSpawnPacketsOfConnectedPlayers())
        {
            _writer.Reset();
            _writer.Put(spawnPacket);
            peer.Send(_writer, DeliveryMethod.ReliableOrdered);
        }
        
        //send everyone new peer PlayerSpawn packet
        _writer.Reset();
        _writer.Put(_packetManager.GetSpawnPacket(peer, packet));
        _server.SendToAll(_writer, DeliveryMethod.ReliableOrdered);
    }

    private void OnSyncPacketReceived(NetPeer sender, INetSerializable packet, DeliveryMethod deliveryMethod)
    {
        _writer.Reset();
        _writer.Put(packet);
        _server.SendToAll(_writer, deliveryMethod, sender);
    }
}