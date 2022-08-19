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
    private Config _config;

    public Client(NetworkSyncService syncService, Config config)
    {
        _syncService = syncService;
        _writer = new NetDataWriter();
        _config = config;
        
        EventBasedNetListener listener = new EventBasedNetListener();
        _client = new NetManager(listener);
        listener.NetworkReceiveEvent += OnNetworkReceiveEvent;
        listener.PeerConnectedEvent += OnPeerConnectedEvent;
    }

    private void OnPeerConnectedEvent(NetPeer peer)
    {
        Debug.WriteLine("Client : connected to server: " + peer.EndPoint);
        _server = peer;
        _writer.Reset();
        _writer.Put(new JoinPacket() {Id = 1});
        _server.Send(_writer, DeliveryMethod.Unreliable);
    }

    private void OnNetworkReceiveEvent(NetPeer peer, NetPacketReader reader, byte channel, DeliveryMethod deliveryMethod)
    {
        var packetType = (PacketType) reader.GetByte();
        switch (packetType)
        {
            case PacketType.Join:
                break;
            case PacketType.JoinAccept:
                _syncService.AcceptJoin(reader.Get<JoinAcceptPacket>());
                _writer.Reset();
                _writer.Put(_syncService.GetPlayerData());
                _server.Send(_writer, DeliveryMethod.Unreliable);
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
        _client.Start(_config.ClientPort);
        Debug.WriteLine($"Client : listening on port {_config.ClientPort}");
        Debug.WriteLine($"Client : connecting to {_config.ServerHost}:{ _config.ServerPort}");
        
        _client.Connect(_config.ServerHost, _config.ServerPort, _config.ConnectionKey);
    }

    public void Update()
    {
        _client.PollEvents();

        if (_server != null)
        {
            _writer.Reset();
            _writer.Put(_syncService.GetPlayerState());
            _server.Send(_writer, DeliveryMethod.Unreliable);
        }
    }

    public void Stop()
    {
        _client.Stop();
        Debug.WriteLine($"Client : stopped");
    }
}