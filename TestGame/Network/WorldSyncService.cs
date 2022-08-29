using Microsoft.Extensions.Logging;
using TestGame.Core;

namespace TestGame.Network;

public class WorldSyncService : ISyncPacketListener
{
    private World _world;
    private ILogger<WorldSyncService> _logger;

    public WorldSyncService(World world, ILogger<WorldSyncService> logger)
    {
        _world = world;
        _logger = logger;
    }

    public void OnSyncPlayerPacketReceived(SyncPlayerPacket packet)
    {
        var player = _world.Players.FindById(packet.PlayerId);
        if (player == null)
        {
            _logger.LogError("Received invalid PlayerSyncPacket. There is no player with id {PlayerId} in the world.", packet.PlayerId);
            return;
        }

        player.Position.X = packet.X;
        player.Position.Y = packet.Y;
        player.Direction.X = packet.DirectionX;
        player.Direction.Y = packet.DirectionY;
    }
}