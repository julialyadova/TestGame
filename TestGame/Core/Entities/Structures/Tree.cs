using Microsoft.Xna.Framework;
using TestGame.Core.Entities.Base;

namespace TestGame.Core.Entities.Structures;

public class Tree : Structure
{
    public Tree()
    {
        Size = new Point(2, 1);
        Height = 3;
        TextureName = "Textures/Structures/Trees/tree";
    }
}