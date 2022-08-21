using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using TestGame.Core.Map;

namespace TestGame.Adapters;

public class PlayerInputAdapter
{
    private PlayerController _playerController;
    private MapToScreenAdapter _screenAdapter;

    public PlayerInputAdapter(IServiceProvider services)
    {
        _playerController = services.GetRequiredService<PlayerController>();
        _screenAdapter = services.GetRequiredService<MapToScreenAdapter>();
    }

    public void Move(Vector2 direction, GameTime gameTime)
    {
        _playerController.Move(direction);
        _screenAdapter.SetMapOffset(-_playerController.Player.Position);
    }
}