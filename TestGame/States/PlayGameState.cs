using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework.Input;
using TestGame.Core;
using TestGame.Drawing;
using TestGame.Network;
using TestGame.UserInput;

namespace TestGame.States;

public abstract class PlayGameState : GameState
{
    private MapDrawer _drawer;
    protected NetworkServiceProvider Network;
    protected World World;
    protected Camera Camera;
    private IZoomInput ZoomInput;

    public PlayGameState(IServiceProvider services)
    {
        _drawer = services.GetRequiredService<MapDrawer>();
        Network = services.GetRequiredService<NetworkServiceProvider>();
        World = services.GetRequiredService<World>();
        Camera = new Camera();
        ZoomInput = services.GetRequiredService<IZoomInput>();
    }

    public override void Enter()
    {
    }

    public override void HandleInputs(float deltaTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            Network.Stop();
            SetState(MainMenuState);
        }

        ZoomInput.UpdateState();
        if (ZoomInput.IsZooming())
        {
            Camera.Zoom(ZoomInput.GetZoomValue() / 5000f);
        }
    }

    public override void Update(float deltaTime)
    {
        Network.Update();
        Network.SyncPlayer(World.PlayerController.Player);
    }

    public override void Draw()
    {
        _drawer.Draw(World, Camera);
    }
    
    public override void DrawUI()
    {
    }
}