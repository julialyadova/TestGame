using Microsoft.Xna.Framework;

namespace TestGame.Drawing;

public abstract class ScreenAdapter
{
    private const int TileSize = 32;

    public static Vector2 GetScreenVector(Vector2 mapVector)
    {
        return mapVector * TileSize;
    }

    public static Vector2 GetMapVector(Vector2 screenVector)
    {
        return screenVector / TileSize;
    }
    
    public static Rectangle GetScreenRect(Rectangle mapRect)
    {
        return new Rectangle
        ( 
            mapRect.X * TileSize,
            mapRect.Y * TileSize,
            mapRect.Width * TileSize,
            mapRect.Height * TileSize
        );
    }
    
    public static Rectangle GetMapRect(Rectangle screenRect)
    {
        return new Rectangle
        ( 
            screenRect.X / TileSize,
            screenRect.Y / TileSize,
            screenRect.Width / TileSize,
            screenRect.Height / TileSize
        );
    }
}