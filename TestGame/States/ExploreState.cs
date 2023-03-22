using TestGame.Core;
using TestGame.Drawing;
using TestGame.Input;
using TestGame.States.Base;

namespace TestGame.States;

public class ExploreState : GameState
{
    public ExploreState(InputService inputService) : base(inputService) { }

    public override void HandleInputs(float deltaTime, World world)
    {
        if (Input.Movement.State != MovementState.NotMoving)
        {
            world.CurrentPlayer.Move(Input.Movement.Direction, deltaTime);
            world.MainCamera.LookAt(ScreenAdapter.GetScreenVector(world.CurrentPlayer.Player.Position));
        }
        
        if (Input.SpecialKeys.IsClicked(SpecialKeys.BuildMode))
        {
            SetState(BuildState);
        }

        if (Input.SpecialKeys.IsClicked(SpecialKeys.Exit))
        {
            world.Exit();
        }

        if (Input.Zoom.IsZooming)
        {
            world.MainCamera.Zoom(Input.Zoom.Value);
        }

        if (Input.Pointer.Action == PointerAction.Click)
        {
            world.Click(Input.Pointer.Position);
        }
    }
}