using System;
namespace TestGame.States.Base;

public abstract class MainState
{
    public static MainState CurrentState { get; private set; }
    protected static MainMenuState MainMenuState { get; private set; }
    protected static StartServerState StartServerState { get; private set; }
    protected static JoinServerState JoinServerState { get; private set; }
    protected static PlayState PlayState { get; private set; }
    
    public static void LoadContent(IServiceProvider services)
    {
        MainMenuState = new MainMenuState(services);
        StartServerState = new StartServerState(services);
        JoinServerState = new JoinServerState(services);
        PlayState = new PlayState(services);

        CurrentState = MainMenuState;
    }

    protected void SetState(MainState state)
    {
        CurrentState = state;
        state.Enter();
    }

    public virtual void Enter() { }
    public virtual void HandleInputs(float deltaTime) { }
    public virtual void Update(float deltaTime) { }
    public virtual void Draw() { }
}