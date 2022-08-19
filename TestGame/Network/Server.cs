﻿using System.Diagnostics;
using LiteNetLib;
using LiteNetLib.Utils;

namespace TestGame.Network;

public class Server
{
    private NetManager _server;
    private NetDataWriter _writer;
    private NetworkSyncService _syncService;
    private Config _config;
    
    public Server(NetworkSyncService syncService, Config config)
    {
        EventBasedNetListener listener = new EventBasedNetListener();
        _server = new NetManager(listener);
        _syncService = syncService;
        _writer = new NetDataWriter();
        _config = config;

        listener.ConnectionRequestEvent += request =>
        {
            if(_server.ConnectedPeersCount < 4 /* max connections */)
                request.AcceptIfKey(_config.ConnectionKey);
            else
                request.Reject();
        };

        listener.PeerConnectedEvent += OnPeerConnectedEvent;
        listener.NetworkReceiveEvent += OnNetworkReceiveEvent;
    }
    
    private void OnPeerConnectedEvent(NetPeer peer)
    {
        Debug.WriteLine("Server : new connection: " + peer.EndPoint);

        _writer.Reset();
        _writer.Put(new JoinAcceptPacket(){Id = 0, MapSeed = _syncService.GetMapSeed()});
        peer.Send(_writer, DeliveryMethod.Unreliable);
        _writer.Reset();
        _writer.Put(_syncService.GetPlayerData());
        peer.Send(_writer, DeliveryMethod.Unreliable);
    }

    private void OnNetworkReceiveEvent(NetPeer peer, NetPacketReader reader, byte channel, DeliveryMethod deliveryMethod)
    {
        Debug.WriteLine("Server : received packet");
        var packetType = (PacketType) reader.GetByte();
        switch (packetType)
        {
            case PacketType.Join:
                _syncService.Join(reader.Get<JoinPacket>());
                break;
            case PacketType.JoinAccept:
                break;
            case PacketType.SpawnPlayer:
                _syncService.SpawnPlayer(reader.Get<SpawnPlayerPacket>());
                break;
            case PacketType.SyncPlayer:
                _syncService.SyncPlayer(reader.Get<SyncPlayerPacket>());
                break;;
        }
        reader.Recycle();
    }

    public void Start()
    {
        _server.Start(_config.ServerPort);
        Debug.WriteLine($"Server : listening on port {_config.ServerPort}");
    }

    public void Update()
    {
        _server.PollEvents();
        _writer.Reset();
        _writer.Put(_syncService.GetPlayerState());
        _server.SendToAll(_writer, DeliveryMethod.Unreliable);
    }

    public void Stop()
    {
        _server.Stop();
        Debug.WriteLine("Server : stopped");
    }
}