﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using LiteNetLib;
using LiteNetLib.Utils;
using Microsoft.Extensions.DependencyInjection;
using TestGame.Core.Entities.Creatures;
using TestGame.Network.Packets;

namespace TestGame.Network;

public class Client : INetworkService
{
    private NetManager _client;
    private NetPeer _server;
    private NetDataWriter _writer;
    private ClientPacketManager _packetManager;
    private Config _config;
    private Action<JoinResult> joinCallback;
    private ISyncPacketListener _syncPacketListener;

    public Client(IServiceProvider services, ISyncPacketListener syncPacketListener)
    {
        _syncPacketListener = syncPacketListener;
        _packetManager = services.GetRequiredService<ClientPacketManager>();
        _config = services.GetRequiredService<Config>();
        
        _writer = new NetDataWriter();
        var listener = new EventBasedNetListener();
        _client = new NetManager(listener);
        listener.PeerConnectedEvent += OnPeerConnectedEvent;
        listener.NetworkReceiveEvent += OnNetworkReceiveEvent;
        listener.PeerDisconnectedEvent += OnPeerDisconnectedEvent;
        _packetManager.OnSyncRequired += SendSyncPacketReliable;
    }

    public void Connect(string host, int port, string key, string username, Action<JoinResult> callback)
    {
        joinCallback = callback;
        Debug.WriteLine($"Client : connecting to server {host}:{port} as {username}");
        _packetManager.UseUsername(username);
        _client.Start(_config.ClientPort);
        _client.Connect(host, port, key);
    }

    public void Update()
    {
        _client.PollEvents();
    }

    public void SendSyncPacket(INetSerializable packet, DeliveryMethod deliveryMethod)
    {
        _writer.Reset();
        _writer.Put(packet);
        _server.Send(_writer, deliveryMethod);
    }

    public void Stop()
    {
        _client.Stop();
        Debug.WriteLine($"Client : disconnected");
    }

    private void OnPeerConnectedEvent(NetPeer peer)
    {
        Debug.WriteLine($"Client : connected to server {peer.EndPoint}");
        _server = peer;
        
        Debug.WriteLine($"Client : sending join request");
        _writer.Reset();
        _writer.Put(_packetManager.GetJoinRequestPacket());
        _server.Send(_writer, DeliveryMethod.ReliableOrdered);
    }

    private void OnNetworkReceiveEvent(NetPeer peer, NetPacketReader reader, byte channel, DeliveryMethod deliveryMethod)
    {
        var packetType = (PacketType) reader.PeekByte();
        switch (packetType)
        {
            case PacketType.JoinAccepted:
                OnJoinAcceptedPacketReceived(reader.Get<JoinAcceptedPacket>());
                break;
            case PacketType.JoinRejected:
                OnJoinRejectedPacketReceived(reader.Get<JoinRejectedPacket>());
                break;
            case PacketType.SpawnPlayer:
                OnSpawnPlayerPacketReceived(reader.Get<SpawnPlayerPacket>());
                break;
            case PacketType.SyncPlayer:
                OnSyncPlayerPacketReceived(reader.Get<SyncPlayerPacket>());
                break;
            case PacketType.PlayerDisconnected:
                OnPlayerDisconnectedPacketReceived(reader.Get<PlayerDisconnectedPacket>());
                break;
            case PacketType.StructureRemoved:
                OnStructureRemovedPacketReceived(reader.Get<StructureRemovedPacket>());
                break;
        }
        reader.Recycle();
    }

    public async Task OnJoinAcceptedPacketReceived(JoinAcceptedPacket packet)
    {
        joinCallback?.Invoke(JoinResult.Accepted);
        joinCallback = null;
        await _packetManager.OnJoinAccepted(packet);
        
        Debug.WriteLine("Client : sending join packet with player data");
        _writer.Reset();
        _writer.Put(_packetManager.GetJoinPacket());
        _server.Send(_writer, DeliveryMethod.Unreliable);
    }

    public void OnJoinRejectedPacketReceived(JoinRejectedPacket packet)
    {
        joinCallback?.Invoke(JoinResult.Rejected);
        joinCallback = null;
        _packetManager.OnJoinRejected(packet);
    }

    public void OnSpawnPlayerPacketReceived(SpawnPlayerPacket packet)
    {
        _packetManager.SpawnPlayer(packet);
    }

    public void OnSyncPlayerPacketReceived(SyncPlayerPacket packet)
    {
        _syncPacketListener.OnSyncPlayerPacketReceived(packet);
    }
    
    private void OnStructureRemovedPacketReceived(StructureRemovedPacket packet)
    {
        _packetManager.OnStructureRemoved(packet);
    }
    
    private void OnPeerDisconnectedEvent(NetPeer peer, DisconnectInfo disconnectinfo)
    {
        _packetManager.Disconnect();
    }
    private void OnPlayerDisconnectedPacketReceived(PlayerDisconnectedPacket packet)
    {
        _packetManager.OnPlayerDiconnected(packet);
    }

    private void SendSyncPacketReliable(INetSerializable packet)
    {
        _writer.Reset();
        _writer.Put(packet);
        _server.Send(_writer, DeliveryMethod.ReliableUnordered);
    }
}