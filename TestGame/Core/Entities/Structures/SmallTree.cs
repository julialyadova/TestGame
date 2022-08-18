using Microsoft.Xna.Framework;

namespace TestGame.Core.Entities.Structures;

public class SmallTree : Tree
{
    public SmallTree()
    {
        Size = new Point(1, 1);
        Height = 1;
    }
}