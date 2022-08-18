using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TestGame.UI;

public interface IGameUI
{
    void Draw(SpriteBatch spriteBatch);
    bool CheckMouseClick(Point clickPosition);
}