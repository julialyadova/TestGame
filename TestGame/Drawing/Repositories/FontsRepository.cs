using Microsoft.Xna.Framework.Graphics;

namespace TestGame.Drawing.Repositories;

public class FontsRepository : ContentRepository
{
    public SpriteFont MainFont;
    public int LetterWidth;
    public int LetterHeight;

    public FontsRepository()
    {
        MainFont = ContentManager.Load<SpriteFont>("Fonts/main");
        LetterWidth = (int) MainFont.MeasureString("A").X;
        LetterHeight = (int) MainFont.MeasureString("A").Y;
    }
}