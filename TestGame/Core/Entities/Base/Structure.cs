using Microsoft.Xna.Framework;

namespace TestGame.Core.Entities.Base;

public abstract class Structure : Entity
{
    public bool CanWalkThrough;
    public Point MapSize { get; protected set; } = new (1,1);
    public int Height { get; protected set; } = 1;
}
