using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using TestGame.Core.Entities.Creatures;
using TestGame.Tools;
using TestGame.UserInput;

namespace TestGame.Adapters;

public class PlayerInputAdapter
{
    private IMoveInput _moveInput;
    private Player _player;
    private MapToScreenAdapter _screenAdapter;
    
    public PlayerInputAdapter(IServiceProvider services)
    {
        _moveInput = services.GetRequiredService<IMoveInput>();
        _player = services.GetRequiredService<Player>();
        _screenAdapter = services.GetRequiredService<MapToScreenAdapter>();
    }

    public void Update(GameTime gameTime)
    {
        _moveInput.UpdateState();
        var direction = _moveInput.GetDirection() * (float)gameTime.ElapsedGameTime.TotalSeconds;
        _player.Move(direction);
        
        _screenAdapter.SetMapOffset(-_player.Position);
        
    }
}