using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace TestGame.Core.Entities.Base;

public abstract class Entity
{
    public string TextureName;
    public Vector2 DrawOrigin = new Vector2(0,-1f);
    public Vector2 DrawSize = new Vector2(1f,1f);
    public Vector2 Position = Vector2.Zero;

    public virtual void OnDestroy()
    {
        Debug.WriteLine($"Entity destroyed - {GetType().Name}");
    }
}