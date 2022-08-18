using System;
using LiteNetLib.Utils;

namespace TestGame.Network;

 public enum PacketType : byte
{
    Join,
    JoinAccept
}

public struct JoinPacket : INetSerializable
{
    public byte Id;
    
    public void Serialize(NetDataWriter writer)
    {
        writer.Put((byte)PacketType.Join);
        writer.Put(Id);
    }

    public void Deserialize(NetDataReader reader)
    {
        Id = reader.GetByte();
    }
}

public struct JoinAcceptPacket : INetSerializable
{
    public byte Id;
    public int MapSeed;
    
    public void Serialize(NetDataWriter writer)
    {
        writer.Put((byte)PacketType.JoinAccept);
        writer.Put(Id);
        writer.Put(MapSeed);
    }

    public void Deserialize(NetDataReader reader)
    {
        Id = reader.GetByte();
        MapSeed = reader.GetInt();
    }
}