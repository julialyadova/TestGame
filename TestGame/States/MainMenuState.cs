using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Xna.Framework.Graphics;
using Myra.Graphics2D.UI;
using TestGame.Drawing;

namespace TestGame.States;

public class MainMenuState : GameState
{
    private Desktop _desktop;
    private TextButton _hostButton;
    private TextButton _joinButton;
    private TextButton _exitButton;
    private MainMenuBgDrawer _bgDrawer;

    public MainMenuState(IServiceProvider services)
    {
        _bgDrawer = new MainMenuBgDrawer();
        Project project;
        using (StreamReader reader = new StreamReader("Content/Layouts/MainMenu.xmmp"))
        {
            string data = reader.ReadToEnd();
            project = Project.LoadFromXml(data);
        }
        _desktop = new Desktop();
        _desktop.Root = project.Root;
        
        _hostButton = _desktop.Root.FindWidgetById("host") as TextButton;
        _hostButton.Click += (s, a) =>
        {
            SetState(HostGameState);
        };
        
        _joinButton = _desktop.Root.FindWidgetById("join") as TextButton;
        _joinButton.Click += (s, a) =>
        {
            SetState(JoinGameState);
        };
        
        _exitButton = _desktop.Root.FindWidgetById("exit") as TextButton;
        _exitButton.Click += (s, a) =>
        {
            services.GetRequiredService<IHostApplicationLifetime>().StopApplication();
        };
    }

    public override void HandleInputs(float deltaTime) { }

    public override void Update(float deltaTime) { }

    public override void Draw()
    {
        _bgDrawer.Draw();
    }
    
    public override void DrawUI()
    {
        _desktop.Render();
    }
}