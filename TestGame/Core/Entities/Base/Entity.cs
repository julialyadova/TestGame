using System.Diagnostics;

namespace TestGame.Core.Entities.Base;

public abstract class Entity
{
    public string TextureName;

    public virtual void OnDestroy()
    {
        Debug.WriteLine($"Entity destroyed - {GetType().Name}");
    }
}