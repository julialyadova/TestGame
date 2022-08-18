using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using TestGame.Core.Entities.Creatures;
using TestGame.Tools;
using TestGame.UserInput;

namespace TestGame.Adapters;

public class PlayerInputAdapter
{
    private Player _player;
    private MapToScreenAdapter _screenAdapter;
    
    public PlayerInputAdapter(IServiceProvider services)
    {
        _player = services.GetRequiredService<Player>();
        _screenAdapter = services.GetRequiredService<MapToScreenAdapter>();
    }

    public void Move(Vector2 direction, GameTime gameTime)
    {
        _player.Move(direction * (float)gameTime.ElapsedGameTime.TotalSeconds);
        _screenAdapter.SetMapOffset(-_player.Position);
    }
}