using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TestGame.Commands;
using TestGame.Network;
using Vector2 = System.Numerics.Vector2;

namespace TestGame.UI;

public class GameUI
{
    private UITexturesRepository _textures;
    private FontsRepository _fonts;
    private List<UIElement> _elements;
    private Vector2 _logTextPosition = new Vector2(10,4);
    private String _logText = "log";

    public GameUI(IServiceProvider services)
    {
        _textures = services.GetRequiredService<UITexturesRepository>();
        _fonts = services.GetRequiredService<FontsRepository>();
        _elements = new();
        
        var button = new Button(new Rectangle(10, 24, 60, 60), "Textures/UI/btn_exit");
        button.SetCommand(services.GetRequiredService<ExitPartyCommand>());
        Add(button);
        
        button = new Button(new Rectangle(80, 24, 60, 60), "Textures/UI/btn_host");
        button.SetCommand(services.GetRequiredService<StartServerCommand>());
        Add(button);
        
        button = new Button(new Rectangle(150, 24, 60, 60), "Textures/UI/btn_join");
        button.SetCommand(services.GetRequiredService<JoinGameCommand>());
        Add(button);
        
    }

    public void Log(string message)
    {
        _logText = message;
    }

    public void Add(UIElement element)
    {
        _elements.Add(element);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var element in _elements)
        {
            spriteBatch.Draw(_textures.GetTexture(element.TextureName), element.Bounds, Color.White);
        }
        spriteBatch.DrawString(_fonts.MainFont, _logText, _logTextPosition, Color.Black);
    }

    public bool CheckClick(Point clickPosition)
    {
        foreach (var element in _elements)
        {
            if (element.Bounds.Contains(clickPosition))
            {
                element.OnClick(clickPosition);
                return true;
            }
        }

        return false;
    }
}