using System;
using Microsoft.Extensions.DependencyInjection;
using TestGame.Core;
using TestGame.Input;
using TestGame.States.Base;
using TestGame.UI.Abstractions;

namespace TestGame.States;

public class BuildState : GameState
{
    private const int CameraMoveSpeed = 5;
    
    private readonly IBuildUI _buildUI;
    
    public BuildState(IServiceProvider services) : base(services.GetRequiredService<InputService>())
    {
        _buildUI = services.GetRequiredService<IBuildUI>();
    }

    public override void HandleInputs(float deltaTime, World world)
    {
        if (Input.Movement.State != MovementState.NotMoving)
        {
            world.MainCamera.Move(Input.Movement.Direction * CameraMoveSpeed);
        }

        if (Input.SpecialKeys.IsClicked(SpecialKeys.BuildMode) || Input.SpecialKeys.IsClicked(SpecialKeys.Exit))
        {
            SetState(ExploreState);
        }
        
        if (Input.Zoom.IsZooming)
        {
            world.MainCamera.Zoom(Input.Zoom.Value);
        }
    }

    public override void Draw()
    {
        _buildUI.Draw();
    }
}