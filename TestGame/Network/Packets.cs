using System;
using LiteNetLib.Utils;

namespace TestGame.Network;

 public enum PacketType : byte
{
    Join,
    JoinAccept,
    SpawnPlayer,
    SyncPlayer
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

public struct SpawnPlayerPacket : INetSerializable
{
    public byte Id;
    public string Name;
    public float X;
    public float Y;

    public void Serialize(NetDataWriter writer)
    {
        writer.Put((byte)PacketType.SpawnPlayer);
        writer.Put(Id);
        writer.Put(Name);
        writer.Put(X);
        writer.Put(Y);
    }

    public void Deserialize(NetDataReader reader)
    {
        Id = reader.GetByte();
        Name = reader.GetString();
        X = reader.GetFloat();
        Y = reader.GetFloat();
    }
}

public struct SyncPlayerPacket : INetSerializable
{
    public byte Id;
    public float X;
    public float Y;

    public void Serialize(NetDataWriter writer)
    {
        writer.Put((byte)PacketType.SyncPlayer);
        writer.Put(Id);
        writer.Put(X);
        writer.Put(Y);
    }

    public void Deserialize(NetDataReader reader)
    {
        Id = reader.GetByte();
        X = reader.GetFloat();
        Y = reader.GetFloat();
    }
}