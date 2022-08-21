using System;
using System.Diagnostics;
using LiteNetLib;
using LiteNetLib.Utils;
using Microsoft.Extensions.DependencyInjection;
using TestGame.Network.Packets;

namespace TestGame.Network;

public class Server
{
    private NetManager _server;
    private NetDataWriter _writer;
    private ServerPacketManager _packetManager;
    private Config _config;

    public Server(IServiceProvider services)
    {
        EventBasedNetListener listener = new EventBasedNetListener();
        _server = new NetManager(listener);
        _writer = new NetDataWriter();

        _packetManager = services.GetRequiredService<ServerPacketManager>();
        _config = services.GetRequiredService<Config>();

        listener.ConnectionRequestEvent += OnConnectionRequestEvent;
        listener.PeerConnectedEvent += OnPeerConnectedEvent;
        listener.PeerDisconnectedEvent += OnPeerDisconnectedEvent;
        listener.NetworkReceiveEvent += OnNetworkReceiveEvent;
    }
    
    public void Start(int port)
    {
        _server.Start(port);
        Debug.WriteLine($"Server : listening on port {port}");
    }

    public void Update()
    {
        _server.PollEvents();
    }

    public void Stop()
    {
        _server.Stop();
        Debug.WriteLine("Server : stopped");
    }

    private void OnConnectionRequestEvent(ConnectionRequest request)
    {
        if(_server.ConnectedPeersCount < _config.MaxConnections)
            request.AcceptIfKey(_config.ConnectionKey);
        else
            request.Reject();
    }

    private void OnPeerConnectedEvent(NetPeer peer)
    {
        Debug.WriteLine("Server : new peer is connecting: " + peer.EndPoint);
    }
    
    private void OnPeerDisconnectedEvent(NetPeer peer, DisconnectInfo disconnectinfo)
    {
        Debug.WriteLine($"Server : peer {peer.Id} {peer.EndPoint} disconnected. { disconnectinfo.Reason.ToString()}");
        
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
                SendPacketToOtherPlayers(peer,reader.Get<SyncPlayerPacket>(), DeliveryMethod.Unreliable);
                break;
            case PacketType.StructureRemoved:
                SendPacketToOtherPlayers(peer, reader.Get<StructureRemovedPacket>(), DeliveryMethod.ReliableUnordered);
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

    private void SendPacketToOtherPlayers(NetPeer sender, INetSerializable packet, DeliveryMethod deliveryMethod)
    {
        _writer.Reset();
        _writer.Put(packet);
        _server.SendToAll(_writer, deliveryMethod, sender);
    }
}