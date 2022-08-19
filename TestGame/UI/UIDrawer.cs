using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TestGame.UI;

public class UIDrawer
{
    private Point _resolution;
    private GameUI _ui;
    private UITexturesRepository _textures;
    private FontsRepository _fonts;
    private Rectangle _drawRect;
    
    public UIDrawer(IServiceProvider services)
    {
        _ui = services.GetRequiredService<GameUI>();
        _textures = services.GetRequiredService<UITexturesRepository>();
        _fonts = services.GetRequiredService<FontsRepository>();
        var config = services.GetRequiredService<Config>();
        _resolution = new Point(config.ScreenWidth, config.ScreenHeight);
    }

    public void BakeUI()
    {
        foreach (var element in _ui.Elements)
        {
            Align(element);
            element.ScreenBounds = _drawRect;
        }
    }

    private void Align(UIElement element)
    {
        _drawRect = element.Bounds;
        switch (element.HorizontalAlignment)
        {
            case HorizontalAlignment.Center:
                _drawRect.X += _resolution.X / 2 - _drawRect.Width / 2;
                break;
            case HorizontalAlignment.Right:
                _drawRect.X += _resolution.X - _drawRect.Height;
                break;
        }

        if (element.VerticalAlignment == VerticalAlignment.Bottom)
        {
            _drawRect.Y = _resolution.Y - _drawRect.Height + _drawRect.Y;
        }
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var element in _ui.Elements)
        {
            if (element.Visible)
            {
                _drawRect = element.ScreenBounds;
                if (element is TextBox textBox)
                    DrawText(textBox, spriteBatch);
                else if (element is Button button)
                    DrawButton(button, spriteBatch);
                else if (element is Panel panel)
                    DrawPanel(panel, spriteBatch);                   
            }
        }
    }

    public void DrawText(TextBox textBox, SpriteBatch spriteBatch)
    {
        switch (textBox.TextAlignment)
        {
            case HorizontalAlignment.Center:
                _drawRect.X += textBox.Bounds.Width / 2;
                _drawRect.X -= textBox.TextLength * _fonts.LetterWidth / 2;
                break;
            case HorizontalAlignment.Right:
                _drawRect.X += textBox.Bounds.Width;
                _drawRect.X -= textBox.TextLength * _fonts.LetterWidth;
                break;
        }
        
        spriteBatch.DrawString(
            _fonts.MainFont,
            textBox.Text,
            _drawRect.Location.ToVector2(),
            textBox.Color);
    }

    public void DrawButton(Button button, SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(
            _textures.GetTexture(button.Backgroundtexture),
            _drawRect,
            Color.White);

        if (button.Text != null)
        {
            _drawRect.X += button.Bounds.Width / 2 - _fonts.LetterWidth / 2 * button.TextLength;
            _drawRect.Y += button.Bounds.Height / 2 - _fonts.LetterHeight / 2;
            spriteBatch.DrawString(
                _fonts.MainFont,
                button.Text,
                _drawRect.Location.ToVector2(),
                button.TextColor);
        }
    }
    
    public void DrawPanel(Panel panel, SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(
            _textures.BlankTexture(),
            _drawRect,
            panel.Color);
    }
}