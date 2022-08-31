using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework.Input;
using TestGame.Core;
using TestGame.Drawing;
using TestGame.States.Base;
using TestGame.UserInput;

namespace TestGame.States;

public class ExploreState : GameState
{
    private readonly IMoveInput _moveInput;
    private readonly KeyboardInput _keysInput;
    private readonly IZoomInput _zoomInput;

    public ExploreState(IServiceProvider services)
    {
        _moveInput = services.GetRequiredService<IMoveInput>();
        _keysInput = services.GetRequiredService<KeyboardInput>();
        _zoomInput = services.GetRequiredService<IZoomInput>();
    }

    public override void HandleInputs(float deltaTime, World world)
    {
        _moveInput.UpdateState();
        if (_moveInput.IsMoving())
        {
            world.PlayerController.Move(_moveInput.GetDirection(), deltaTime);
            world.MainCamera.LookAt(ScreenAdapter.GetScreenVector(world.PlayerController.Player.Position));
        }
        
        _keysInput.UpdateState();
        if (_keysInput.IsKeyFirstPressed(Keys.B))
        {
            SetState(BuildState);
        }
        
        _zoomInput.UpdateState();
        if (_zoomInput.IsZooming())
        {
            world.MainCamera.Zoom(_zoomInput.GetZoomValue());
        }
    }
}