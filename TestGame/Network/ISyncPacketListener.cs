namespace TestGame.Network;

public interface ISyncPacketListener
{
    public void OnSyncPlayerPacketReceived(SyncPlayerPacket packet);
}