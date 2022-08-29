using LiteNetLib;
using LiteNetLib.Utils;
using TestGame.Core.Entities.Creatures;

namespace TestGame.Network;

public interface INetworkService
{
    public void Update();
    public void SendSyncPacket(INetSerializable packet, DeliveryMethod deliveryMethod);
    public void Stop();
}