using Microsoft.Xna.Framework;
using TestGame.Core.Entities.Base;

namespace TestGame.Core.Entities.Creatures;

public class Player : Entity
{
    public const int MoveSpeed = 5;
    
    public byte Id;
    public string Name;
    public Vector2 Direction = Vector2.Zero;
    public Vector2 Size = new Vector2(1, 2);

    public Player()
    {
        DrawOrigin = new Vector2(0, 0);
        DrawSize = new Vector2(2, 4);
        Anchor = new Vector2(0.5f, 1);
    }

    public bool LooksRight()
    {
        return Direction.X > 0;
    }

    public bool LooksLeft()
    {
        return Direction.X < 0;
    }
    
    public bool LooksForward()
    {
        return Direction.Y >= 0;
    }
    
    public bool LooksBack()
    {
        return Direction.Y < 0;
    }
}