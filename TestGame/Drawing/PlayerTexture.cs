using Microsoft.Xna.Framework.Graphics;

namespace TestGame.Drawing;

public class PlayerTexture
{
    public readonly Texture2D Front;
    public readonly Texture2D Back;
    public readonly Texture2D Side;

    public PlayerTexture(Texture2D front, Texture2D back, Texture2D side)
    {
        Front = front;
        Back = back;
        Side = side;
    }
}