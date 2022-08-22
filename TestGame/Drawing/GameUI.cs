using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Myra.Graphics2D.UI;
using TestGame.Commands;

namespace TestGame.Drawing;

public class GameUI
{
    private Config _config;
    private IServiceProvider _services;
    private Desktop _desktop;
    private Project _mainLayout;
    private TextButton _menuButton;
    
    private Window _menuWindow;
    private TextButton _exitButton;
    private TextButton _hostButton;
    private TextButton _joinButton;
    private TextButton _leaveButton;
    private Label _log;
    private Label _fps;

    public GameUI(IServiceProvider services)
    {
        _services = services;
        _config = services.GetRequiredService<Config>();
    }
    public void LoadContent()
    {
        LoadProject();
        _desktop = new Desktop();
        _desktop.Root = _mainLayout.Root;

        _menuButton.Click += (s, a) => _menuWindow.ShowModal(_desktop);
        _exitButton.Click += (s, a) => _services.GetRequiredService<ExitGameCommand>().Execute();
        _hostButton.Text = $"Host game on port {_config.ServerPort}";
        _hostButton.Click += (s, a) =>
        {
            _services.GetRequiredService<HostGameCommand>().Execute();
            _hostButton.Enabled = false;
            _joinButton.Enabled = false;
            _leaveButton.Text = "Stop server and leave map";
            _leaveButton.Visible = true;
        };
        _joinButton.Text = $"Join server {_config.ServerHost}:{_config.ServerPort}";
        _joinButton.Click += (s, a) =>
        {
            _services.GetRequiredService<JoinGameCommand>().Execute();
            _hostButton.Enabled = false;
            _joinButton.Enabled = false;
            _leaveButton.Text = "Leave server";
            _leaveButton.Visible = true;
        };
        _leaveButton.Click += (s, a) =>
        {
            _services.GetRequiredService<DisconnectCommand>().Execute();
            _leaveButton.Visible = false;
            _joinButton.Enabled = true;
            _hostButton.Enabled = true;
        };
    }

    private void LoadProject()
    {
        using (StreamReader reader = new StreamReader("Content/Layouts/layout.xmmp"))
        {
            string data = reader.ReadToEnd();
            _mainLayout = Project.LoadFromXml(data);
        }
        _menuButton = _mainLayout.Root.FindWidgetById("menu") as TextButton;
        _log = _mainLayout.Root.FindWidgetById("log") as Label;
        _fps = _mainLayout.Root.FindWidgetById("fps") as Label;

        Project menu;
        using (StreamReader reader = new StreamReader("Content/Layouts/menu.xmmp"))
        {
            string data = reader.ReadToEnd();
            menu = Project.LoadFromXml(data);
           
        }
        _menuWindow = menu.Root as Window;
        _exitButton = menu.Root.FindWidgetById("exit") as TextButton;
        _hostButton = menu.Root.FindWidgetById("host") as TextButton;
        _joinButton = menu.Root.FindWidgetById("join") as TextButton;
        _leaveButton = menu.Root.FindWidgetById("leave") as TextButton;
    }

    public void ShowMessage(string message)
    {
        if (_log != null) 
            _log.Text = message;
    }

    public void ShowFPS(int fps)
    {
        _fps.Text = fps.ToString();
    }
    
    public void Draw()
    {
        _desktop.Render();
    }
}