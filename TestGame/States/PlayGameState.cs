using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework.Input;
using TestGame.Core;
using TestGame.Drawing;
using TestGame.Network;

namespace TestGame.States;

public abstract class PlayGameState : GameState
{
    private MapDrawer _drawer;
    protected NetworkServiceProvider Network;
    protected World World;
    protected Camera Camera;

    public PlayGameState(IServiceProvider services)
    {
        _drawer = services.GetRequiredService<MapDrawer>();
        Network = services.GetRequiredService<NetworkServiceProvider>();
        World = services.GetRequiredService<World>();
        Camera = new Camera();
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