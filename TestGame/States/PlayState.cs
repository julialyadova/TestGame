using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework.Input;
using TestGame.Core;
using TestGame.Drawing;
using TestGame.Network;
using TestGame.States.Base;

namespace TestGame.States;

public class PlayState : MainState
{
    private readonly World _world;
    private readonly MapDrawer _worldDrawer;
    private readonly NetworkServiceProvider _network;
    
    public PlayState(IServiceProvider services)
    {
        _world = services.GetRequiredService<World>();
        _worldDrawer = services.GetRequiredService<MapDrawer>();
        _network = services.GetRequiredService<NetworkServiceProvider>();

        _world.OnExit = () => SetState(MainMenuState);
    }

    public override void HandleInputs(float deltaTime)
    {
        GameState.CurrentState.HandleInputs(deltaTime, _world);
    }

    public override void Update(float deltaTime)
    {
        _network.Update();
        _network.SyncPlayer(_world.CurrentPlayer.Player);
    }

    public override void Draw()
    {
        _worldDrawer.Draw(_world);
        GameState.CurrentState.Draw();
    }
}