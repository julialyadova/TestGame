using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework.Input;
using TestGame.Core;
using TestGame.States.Base;
using TestGame.UI.Abstractions;
using TestGame.UserInput;

namespace TestGame.States;

public class BuildState : GameState
{
    private const int CameraMoveSpeed = 5;
    
    private readonly IMoveInput _moveInput;
    private readonly KeyboardInput _keysInput;
    private readonly IZoomInput _zoomInput;
    private readonly IBuildUI _buildUI;
    
    public BuildState(IServiceProvider services)
    {
        _moveInput = services.GetRequiredService<IMoveInput>();
        _keysInput = services.GetRequiredService<KeyboardInput>();
        _zoomInput = services.GetRequiredService<IZoomInput>();
        _buildUI = services.GetRequiredService<IBuildUI>();
    }

    public override void HandleInputs(float deltaTime, World world)
    {
        _moveInput.UpdateState();
        if (_moveInput.IsMoving())
        {
            world.MainCamera.Move(_moveInput.GetDirection() * CameraMoveSpeed);
        }
        
        _keysInput.UpdateState();
        if (_keysInput.IsKeyFirstPressed(Keys.Escape) || _keysInput.IsKeyFirstPressed(Keys.B))
        {
            SetState(ExploreState);
        }
        
        _zoomInput.UpdateState();
        if (_zoomInput.IsZooming())
        {
            world.MainCamera.Zoom(_zoomInput.GetZoomValue());
        }
    }

    public override void Draw()
    {
        _buildUI.Draw();
    }
}