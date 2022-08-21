using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using TestGame.Core.Map;
using TestGame.UserInput;

namespace TestGame.Adapters;

public class PlayerInputAdapter
{
    private const int PlayerSpeed = 5;
    private readonly PlayerController _playerController;
    private readonly MapToScreenAdapter _screenAdapter;

    public PlayerInputAdapter(IServiceProvider services)
    {
        _playerController = services.GetRequiredService<PlayerController>();
        _screenAdapter = services.GetRequiredService<MapToScreenAdapter>();
    }

    public void Move(Vector2 direction, GameTime gameTime)
    {
        _playerController.Move(direction, (float)gameTime.ElapsedGameTime.TotalSeconds);
        _screenAdapter.SetMapOffset(-_playerController.Player.Position);
    }
    
    public void OnControlPressed(GameControl control)
    {
        if (control == GameControl.Interact)
            _playerController.Interact();
    }
}