using Microsoft.Xna.Framework;
using TestGame.Core.Entities.Base;

namespace TestGame.Core.Entities.Creatures;

public class Player : Entity
{
    public byte Id;
    public string Name;
    public Vector2 Position;
    public Vector2 Size = new Vector2(1, 2);
}