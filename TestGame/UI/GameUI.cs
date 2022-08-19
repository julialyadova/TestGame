using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using TestGame.Commands;


namespace TestGame.UI;

public class GameUI
{
    public List<UIElement> Elements = new();
    private Button _exitButton;

    public GameUI(IServiceProvider services)
    {
        var config = services.GetRequiredService<Config>();
        
        var button = new Button(UIId.ExitButton);
        button.Command = services.GetRequiredService<ExitGameCommand>();
        button.Bounds = new Rectangle(-10, 10, 40, 40);
        button.SetText("Exit");
        button.TextColor = Color.Red;
        button.HorizontalAlignment = HorizontalAlignment.Right;
        Elements.Add(button);
        
        var text = new TextBox(UIId.ConnectionStatus);
        text.Bounds = new Rectangle(10, 10, 100, 30);
        text.SetText("NotConnected");
        Elements.Add(text);
        
        button = new Button(UIId.HostButton);
        button.Command = services.GetRequiredService<HostGameCommand>();
        button.Bounds = new Rectangle(10, 40, 220, 28);
        button.SetText("Host Game");
        Elements.Add(button);
        
        button = new Button(UIId.JoinButton);
        button.Command = services.GetRequiredService<JoinGameCommand>();
        button.Bounds = new Rectangle(10, 80, 220, 28);
        button.SetText($"Join {config.ServerHost}:{config.ServerPort}");
        Elements.Add(button);
        
        button = new Button(UIId.DisconnectButton);
        button.Command = services.GetRequiredService<DisconnectCommand>();
        button.Bounds = new Rectangle(10, 120, 220, 28);
        button.SetText("Disconnect");
        Elements.Add(button);

        text = new TextBox(UIId.Message);
        text.Bounds = new Rectangle(0, 10, 0, 0);
        text.SetText($"Welcome, {config.PlayerName}!");
        text.Color = Color.White;
        text.TextAlignment = HorizontalAlignment.Center;
        text.HorizontalAlignment = HorizontalAlignment.Center;
        Elements.Add(text);
    }

    public T GetElement<T>(UIId id) where T : UIElement
    {
        return Elements.Find(e => e.Id == id) as T;
    }

    public bool CheckClick(Point clickPosition)
    {
        foreach (var element in Elements)
            if (element.Visible && element.ScreenBounds.Contains(clickPosition))
            {
                element.OnClick(clickPosition);
                return true;
            }

        return false;
    }
}