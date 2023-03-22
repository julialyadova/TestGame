using Microsoft.Extensions.DependencyInjection;
using System;
using TestGame.Core;
using TestGame.Input;

namespace TestGame.States.Base;

public abstract class GameState
{
    public static GameState CurrentState { get; private set; }
    protected static GameState ExploreState { get; private set; }
    protected static GameState BuildState { get; private set; }

    protected readonly InputService Input;

    public GameState(InputService inputService)
    {
        Input = inputService;
    }


    public static void LoadContent(IServiceProvider services)
    {
        ExploreState = new ExploreState(services.GetRequiredService<InputService>());
        BuildState = new BuildState(services);
        
        CurrentState = ExploreState;
    }

    protected static void SetState(GameState state)
    {
        CurrentState = state;
        state.Enter();
    }

    public virtual void Enter() { }
    public virtual void HandleInputs(float deltaTime, World world) { }
    public virtual void Update(float deltaTime, World world) { }
    public virtual void Draw() { }
}