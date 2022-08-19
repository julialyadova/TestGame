using Microsoft.Xna.Framework;
using TestGame.Core.Entities.Base;

namespace TestGame.Core.Entities.Structures;

public class HighTree : Structure
{
    public HighTree()
    {
        Size = new Point(4, 2);
        Height = 10;
        TextureName = "Textures/Structures/Trees/high_tree";
    }
}