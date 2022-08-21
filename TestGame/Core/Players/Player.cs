using Microsoft.Xna.Framework;
using TestGame.Core.Entities.Base;

namespace TestGame.Core.Entities.Creatures;

public class Player : Entity
{
    public const int MoveSpeed = 5;
    
    public byte Id;
    public string Name;
    public Vector2 Position= Vector2.One;
    public Vector2 Direction = Vector2.Zero;
    public Vector2 Size = new Vector2(1, 2);
}