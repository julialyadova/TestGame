using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TestGame.Core.Entities.Base;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace TestGame.Core.Entities.Structures;

public class Wall : Structure
{
    public Wall Left;
    public Wall Right;
    public Wall(int height)
    {
        Height = height;
        TextureName = "Textures/Structures/brick_wall";
    }

    public void Connect(Wall wall)
    {
        if (wall.Position.X == Position.X + 1)
        {
            Right = wall;
            wall.Left = this;
        }
        else if (wall.Position.X == Position.X - 1)
        {
            Left = wall;
            wall.Right = this; 
        }
    }
}