using TestGame.Core.Entities.Base;

namespace TestGame.Core.Entities.Surfaces;

public class Water : Surface
{
    public Water()
    {
        CanBuild = false;
    }
}