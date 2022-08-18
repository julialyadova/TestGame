using System.Diagnostics;
using LiteNetLib;
using LiteNetLib.Utils;

namespace TestGame.Network;

public class Client
{
    private NetManager _client;
    private NetPeer _server;
    private NetDataWriter _writer;
    private NetworkSyncService _syncService;

    public Client( NetworkSyncService syncService)
    {
        _syncService = syncService;
        _writer = new NetDataWriter();
        
        EventBasedNetListener listener = new EventBasedNetListener();
        _client = new NetManager(listener);
        listener.NetworkReceiveEvent += OnNetworkReceiveEvent;
        listener.PeerConnectedEvent += OnPeerConnectedEvent;
    }

    private void OnPeerConnectedEvent(NetPeer peer)
    {
        Debug.WriteLine("Client connected to server: " + peer.EndPoint);
        _server = peer;
        _writer.Reset();
        _writer.Put(new JoinPacket() {Id = 1});
        _server.Send(_writer, DeliveryMethod.Unreliable);
    }

    private void OnNetworkReceiveEvent(NetPeer peer, NetPacketReader reader, byte channel, DeliveryMethod deliverymethod)
    {
        var packetType = (PacketType) reader.GetByte();
        switch (packetType)
        {
            case PacketType.Join:
                break;
            case PacketType.JoinAccept:
                _syncService.AcceptJoin(reader.Get<JoinAcceptPacket>());
                break;
        }
        reader.Recycle();
    }

    public void Start(int port)
    {
        _client.Start(port);
        Debug.WriteLine($"Client is listening on port {port}");
        Debug.WriteLine($"Client is connecting to localhost:{port}");
        _client.Connect("localhost", 11037, "Bat");
    }

    public void Update()
    {
        _client.PollEvents();
    }

    public void Stop()
    {
        _client.Stop();
        Debug.WriteLine($"Client stopped");
    }
}