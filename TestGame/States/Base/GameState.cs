using System;
using TestGame.Core;

namespace TestGame.States.Base;

public abstract class GameState
{
    public static GameState CurrentState { get; private set; }
    protected static GameState ExploreState { get; private set; }
    protected static GameState BuildState { get; private set; }


    public static void LoadContent(IServiceProvider services)
    {
        ExploreState = new ExploreState(services);
        BuildState = new BuildState(services);
        
        CurrentState = ExploreState;
    }

    protected void SetState(GameState state)
    {
        CurrentState = state;
        state.Enter();
    }

    public virtual void Enter() { }
    public virtual void HandleInputs(float deltaTime, World world) { }
    public virtual void Update(float deltaTime, World world) { }
    public virtual void Draw() { }
}