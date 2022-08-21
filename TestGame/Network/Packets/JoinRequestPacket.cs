using LiteNetLib.Utils;

namespace TestGame.Network.Packets;

public struct JoinRequestPacket : INetSerializable
{
    public string Username;

    public void Serialize(NetDataWriter writer)
    {
        writer.Put((byte)PacketType.RequestJoin);
        writer.Put(Username);
    }

    public void Deserialize(NetDataReader reader)
    {
        reader.GetByte();
        Username = reader.GetString();
    }
}