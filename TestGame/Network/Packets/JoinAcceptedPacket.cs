using LiteNetLib.Utils;

namespace TestGame.Network.Packets;

public struct JoinAcceptedPacket : INetSerializable
{
    public byte PlayerId;
    public int MapSeed; //todo: send map data not only seed
    
    public void Serialize(NetDataWriter writer)
    {
        writer.Put((byte)PacketType.JoinAccepted);
        writer.Put(PlayerId);
        writer.Put(MapSeed);
    }

    public void Deserialize(NetDataReader reader)
    {
        reader.GetByte();
        PlayerId = reader.GetByte();
        MapSeed = reader.GetInt();
    }
}