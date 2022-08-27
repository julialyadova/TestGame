using System;
using Microsoft.Xna.Framework.Graphics;

namespace TestGame.States;

public abstract class GameState
{
    public static SpriteBatch SpriteBatch { get; set; }
    public static GameState CurrentState { get; private set; }
    protected static MainMenuState MainMenuState;
    protected static HostGameState HostGameState;
    protected static JoinGameState JoinGameState;
    protected static ExploreWorldState ExploreWorldState;
    protected static BuildState BuildState;


    public static void LoadContent(IServiceProvider services)
    {
        MainMenuState = new MainMenuState(services);
        HostGameState = new HostGameState(services);
        JoinGameState = new JoinGameState(services);
        ExploreWorldState = new ExploreWorldState(services);
        BuildState = new BuildState(services);
        CurrentState = MainMenuState;
    }

    protected void SetState(GameState state)
    {
        CurrentState = state;
        state.Enter();
    }

    public virtual void Enter() { }

    public abstract void HandleInputs(float deltaTime);
    public abstract void Update(float deltaTime);
    public abstract void Draw();
    public abstract void DrawUI();
}