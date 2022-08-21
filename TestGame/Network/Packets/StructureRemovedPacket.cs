using LiteNetLib.Utils;

namespace TestGame.Network.Packets;

public struct StructureRemovedPacket : INetSerializable
{
    public int X;
    public int Y;
    
    public void Serialize(NetDataWriter writer)
    {
        writer.Put((byte)PacketType.StructureRemoved);
        writer.Put(X);
        writer.Put(Y);
    }

    public void Deserialize(NetDataReader reader)
    {
        reader.GetByte();
        X = reader.GetInt();
        Y = reader.GetInt();
    }
}