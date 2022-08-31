using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Myra.Graphics2D.UI;
using TestGame.Drawing;
using TestGame.States.Base;

namespace TestGame.States;

public class MainMenuState : MainState
{
    private Desktop _desktop;
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
        
        var hostButton = _desktop.Root.FindWidgetById("host") as TextButton;
        hostButton.Click += (_, _) =>
        {
            SetState(StartServerState);
        };
        
        var joinButton = _desktop.Root.FindWidgetById("join") as TextButton;
        joinButton.Click += (_, _) =>
        {
            SetState(JoinServerState);
        };
        
        var exitButton = _desktop.Root.FindWidgetById("exit") as TextButton;
        exitButton.Click += (_, _) =>
        {
            services.GetRequiredService<IHostApplicationLifetime>().StopApplication();
        };
    }

    public override void Draw()
    {
        _bgDrawer.Draw();
        _desktop.Render();
    }
}