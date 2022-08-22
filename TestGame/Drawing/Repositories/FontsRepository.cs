using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TestGame.Drawing.Repositories;

public class FontsRepository : IContentRepository
{
    public SpriteFont MainFont;
    public int LetterWidth;
    public int LetterHeight;

    public void LoadContent(GraphicsDevice device, ContentManager contentManager)
    {
        MainFont = contentManager.Load<SpriteFont>("Fonts/main");
        LetterWidth = (int) MainFont.MeasureString("A").X;
        LetterHeight = (int) MainFont.MeasureString("A").Y;
    }
}