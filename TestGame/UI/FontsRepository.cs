using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TestGame.UI;

public class FontsRepository
{
    public SpriteFont MainFont;

    public void LoadContent(GraphicsDevice device, ContentManager contentManager)
    {
        MainFont = contentManager.Load<SpriteFont>("Fonts/main");
    }
}