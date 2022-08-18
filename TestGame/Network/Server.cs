using System;
using System.Diagnostics;
using System.Threading;
using LiteNetLib;
using LiteNetLib.Utils;

namespace TestGame.Network;

public class Server
{
    private NetManager _server;
    private NetDataWriter _writer;
    private NetworkSyncService _syncService;
    
    public Server(NetworkSyncService syncService)
    {
        EventBasedNetListener listener = new EventBasedNetListener();
        _server = new NetManager(listener);
        _syncService = syncService;
        _writer = new NetDataWriter();

        listener.ConnectionRequestEvent += request =>
        {
            if(_server.ConnectedPeersCount < 4 /* max connections */)
                request.AcceptIfKey("Bat");
            else
                request.Reject();
        };

        listener.PeerConnectedEvent += OnPeerConnectedEvent;
        listener.NetworkReceiveEvent += OnNetworkReceiveEvent;
    }
    
    private void OnPeerConnectedEvent(NetPeer peer)
    {
        Debug.WriteLine("Server - new connection: " + peer.EndPoint);

        _writer.Reset();
        _writer.Put(new JoinAcceptPacket(){Id = 0, MapSeed = _syncService.GetMapSeed()});
        peer.Send(_writer, DeliveryMethod.Unreliable);
    }

    private void OnNetworkReceiveEvent(NetPeer peer, NetPacketReader reader, byte channel, DeliveryMethod deliverymethod)
    {
        Debug.WriteLine("Server received packet");
        var packetType = (PacketType) reader.GetByte();
        switch (packetType)
        {
            case PacketType.Join:
                _syncService.Join(reader.Get<JoinPacket>());
                break;
            case PacketType.JoinAccept:
                break;
        }
        reader.Recycle();
    }

    public void Start(int port)
    {
        _server.Start(port);
        Debug.WriteLine($"Server is listening on port {port}");
    }

    public void Update()
    {
        _server.PollEvents();
    }

    public void Stop()
    {
        _server.Stop();
    }
}