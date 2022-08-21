using LiteNetLib.Utils;

namespace TestGame.Network.Packets;

public struct PlayerDisconnectedPacket : INetSerializable
{
    public byte PlayerId;

    public void Serialize(NetDataWriter writer)
    {
        writer.Put((byte)PacketType.PlayerDisconnected);
        writer.Put(PlayerId);
    }

    public void Deserialize(NetDataReader reader)
    {
        reader.GetByte();
        PlayerId = reader.GetByte();
    }
}