using LiteNetLib.Utils;

namespace TestGame.Network.Packets;

public struct JoinRejectedPacket : INetSerializable
{
    public string Reason;

    public JoinRejectedPacket(string reason)
    {
        Reason = reason;
    }

    public void Serialize(NetDataWriter writer)
    {
        writer.Put((byte)PacketType.JoinAccepted);
        writer.Put(Reason);
    }

    public void Deserialize(NetDataReader reader)
    {
        reader.GetByte();
        Reason = reader.GetString();
    }
}