using Microsoft.Xna.Framework;
using TestGame.Core.Entities.Base;

namespace TestGame.Core.Entities.Structures;

public class Farm : Structure
{
    public string XConnectionTexture = "Textures/Structures/Farm/farm_connect_x";
    public string YConnectionTexture = "Textures/Structures/Farm/farm_connect_y";
    public string CornerTexture = "Textures/Structures/Farm/farm_corner";
    public Farm()
    {
        DrawOrigin = Vector2.Zero;
        Anchor = Vector2.Zero;
        MapSize = new Point(3, 3);
        DrawSize = MapSize.ToVector2();
        TextureName = "Textures/Structures/Farm/farm";
        CanWalkThrough = true;
        Height = 0;
    }
}