using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TestGame.UI;

public class GameUI
{
    private Texture2D _blankTexture;
    private GraphicsDeviceManager _graphics;
    private List<UIElement> _elements;

    public GameUI(GraphicsDeviceManager graphics)
    {
        _graphics = graphics;
        _elements = new();
    }

    public void Add(UIElement element)
    {
        _elements.Add(element);
    }

    public void LoadContent(ContentManager contentManager)
    {
        _blankTexture = new Texture2D(_graphics.GraphicsDevice, 1, 1);
        _blankTexture.SetData(new[] {new Color(Color.White, 0.2f)});
        
        foreach (var element in _elements)
        {
            if (element.TextureName == null)
                element.Texture = _blankTexture;
            else
                element.Texture = contentManager.Load<Texture2D>(element.TextureName);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var element in _elements)
        {
            spriteBatch.Draw(element.Texture, element.Bounds, Color.White);
        }
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