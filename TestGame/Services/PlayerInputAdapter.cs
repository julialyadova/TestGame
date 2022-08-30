using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using TestGame.Core.Players;
using TestGame.Drawing;
using TestGame.UserInput;

namespace TestGame.Services;

public class PlayerInputAdapter
{
    private const int PlayerSpeed = 5;
    private readonly PlayerController _playerController;
    private readonly ScreenAdapter _screenAdapter;

    public PlayerInputAdapter(IServiceProvider services)
    {
        _playerController = services.GetRequiredService<PlayerController>();
        _screenAdapter = services.GetRequiredService<ScreenAdapter>();
    }

    public void Move(Vector2 direction, GameTime gameTime)
    {
        _playerController.Move(direction, (float)gameTime.ElapsedGameTime.TotalSeconds);
    }
    
    public void OnControlPressed(GameControl control)
    {
        if (control == GameControl.Interact)
            _playerController.Interact();
    }
}