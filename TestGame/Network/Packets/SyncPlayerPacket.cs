using LiteNetLib.Utils;
using TestGame.Network.Packets;

namespace TestGame.Network;

public struct SyncPlayerPacket : INetSerializable
{
    public byte PlayerId;
    public float X;
    public float Y;
    public float DirectionX;
    public float DirectionY;

    public void Serialize(NetDataWriter writer)
    {
        writer.Put((byte)PacketType.SyncPlayer);
        writer.Put(PlayerId);
        writer.Put(X);
        writer.Put(Y);
        writer.Put(DirectionX);
        writer.Put(DirectionY);
    }

    public void Deserialize(NetDataReader reader)
    {
        reader.GetByte();
        PlayerId = reader.GetByte();
        X = reader.GetFloat();
        Y = reader.GetFloat();
        DirectionX = reader.GetFloat();
        DirectionY = reader.GetFloat();
    }
}