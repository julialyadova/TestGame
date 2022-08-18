using TestGame.Core.Entities.Base;

namespace TestGame.Core.Entities.Surfaces;

public class Grass : Surface
{
    public Grass()
    {
        TextureName = "Textures/World/grass";
        CanBuild = true;
    }
}