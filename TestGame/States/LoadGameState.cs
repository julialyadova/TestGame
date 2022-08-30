using System;
using System.IO;
using Myra.Graphics2D.UI;

namespace TestGame.States;

public abstract class LoadGameState : GameState
{
    private Desktop _desktop;
    private Label _loadingLabel;
    private Label _messageLabel;
    private float time = 0;
    private int dotsCount = 1;

    public LoadGameState(IServiceProvider services)
    {
        Project project;
        using (StreamReader reader = new StreamReader("Content/Layouts/Loading.xmmp"))
        {
            string data = reader.ReadToEnd();
            project = Project.LoadFromXml(data);
        }
        _desktop = new Desktop();
        _desktop.Root = project.Root;
        
        _loadingLabel = _desktop.Root.FindWidgetById("loading") as Label;
        _messageLabel = _desktop.Root.FindWidgetById("message") as Label;
    }
    
    public override void Update(float deltaTime)
    {
        time += deltaTime;
        if (time > 1)
        {
            time = 0;
            AnimateLoading();
        }
    }

    protected void ShowLoadingMessage(string message)
    {
        _messageLabel.Text = message;
    }

    public override void HandleInputs(float deltaTime) { }

    public override void Draw() { }

    public override void DrawUI()
    {
        _desktop.Render();
    }

    private void AnimateLoading()
    {
        _loadingLabel.Text = "loading" + new string('.', dotsCount);
        
        dotsCount++;
        if (dotsCount == 4)
            dotsCount = 1;
    }
}