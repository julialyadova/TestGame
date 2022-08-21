using TestGame.Core.Entities.Base;

namespace TestGame.Core.Entities.Structures;

public class Farm : Structure
{
    public string XConnectionTexture = "Textures/Structures/Farm/farm_connect_x";
    public string YConnectionTexture = "Textures/Structures/Farm/farm_connect_y";
    public string CornerTexture = "Textures/Structures/Farm/farm_corner";
    public Farm()
    {
        TextureName = "Textures/Structures/Farm/farm";
        CanWalkThrough = true;
        Height = 0;
    }
}