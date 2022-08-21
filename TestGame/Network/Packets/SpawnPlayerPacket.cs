using LiteNetLib.Utils;

namespace TestGame.Network.Packets;

public struct SpawnPlayerPacket : INetSerializable
{
    public byte PlayerId;
    public string Name;
    public string Texture;
    public float X;
    public float Y;

    public void Serialize(NetDataWriter writer)
    {
        writer.Put((byte)PacketType.SpawnPlayer);
        writer.Put(PlayerId);
        writer.Put(Name);
        writer.Put(Texture);
        writer.Put(X);
        writer.Put(Y);
    }

    public void Deserialize(NetDataReader reader)
    {
        reader.GetByte();
        PlayerId = reader.GetByte();
        Name = reader.GetString();
        Texture = reader.GetString();
        X = reader.GetFloat();
        Y = reader.GetFloat();
    }
}