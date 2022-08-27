using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework.Input;
using TestGame.Drawing;
using TestGame.UserInput;

namespace TestGame.States;

public class ExploreWorldState : PlayGameState
{
    private readonly IMoveInput _moveInput;
    private readonly MapToScreenAdapter _screenAdapter;

    public ExploreWorldState(IServiceProvider services) : base(services)
    {
        _moveInput = services.GetRequiredService<IMoveInput>();
        _screenAdapter = services.GetRequiredService<MapToScreenAdapter>();
    }
    
    public override void HandleInputs(float deltaTime)
    {
        base.HandleInputs(deltaTime);

        _moveInput.UpdateState();
        if (_moveInput.IsMoving())
            World.PlayerController.Move(_moveInput.GetDirection(), deltaTime);
        
        if (Keyboard.GetState().IsKeyDown(Keys.B))
            SetState(BuildState);
    }

    public override void Update(float deltaTime)
    {
        _screenAdapter.CenterMap(World.PlayerController.Player.Position);
    }
}