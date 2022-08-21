using LiteNetLib.Utils;

namespace TestGame.Network.Packets;

public struct JoinPacket : INetSerializable
{
    public string Username;
    public string Texture;
    
    public void Serialize(NetDataWriter writer)
    {
        writer.Put((byte)PacketType.Join);
        writer.Put(Username);
        writer.Put(Texture);
    }

    public void Deserialize(NetDataReader reader)
    {
        reader.GetByte();
        Username = reader.GetString();
        Texture = reader.GetString();
    }
}