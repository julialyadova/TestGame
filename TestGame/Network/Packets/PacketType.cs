namespace TestGame.Network.Packets;

 public enum PacketType : byte
{
    RequestJoin,
    JoinAccepted,
    JoinRejected,
    Join,
    SpawnPlayer,
    SyncPlayer,
    PlayerDisconnected
}