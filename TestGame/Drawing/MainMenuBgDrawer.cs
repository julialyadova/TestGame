using Microsoft.Xna.Framework;

namespace TestGame.Drawing;

public class MainMenuBgDrawer : GameDrawer
{
    private Color _color = Color.CornflowerBlue;
    private bool _moreRed;
    
    public void Draw()
    {
        SpriteBatch.GraphicsDevice.Clear(_color);

        if (_color.R >= 200)
            _moreRed = false;
        if (_color.R == 0)
            _moreRed = true;

        if (_moreRed)
            _color.R++;
        else
            _color.R--;
    }
}