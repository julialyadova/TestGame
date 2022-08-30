using Microsoft.Xna.Framework;
using TestGame.Core.Entities.Base;

namespace TestGame.Core.Entities.Structures;

public class Tree : Structure
{
    public Tree()
    {
        DrawOrigin = new Vector2(-1f,-6f);
        DrawSize = new Vector2(4f,8f);
        MapSize = new Point(2,2);
        Height = 3;
        TextureName = "Textures/Structures/Trees/tree";
    }
}