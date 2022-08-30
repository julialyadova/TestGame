using Microsoft.Xna.Framework;
using TestGame.Core.Entities.Base;

namespace TestGame.Core.Entities.Structures;

public class Tree : Structure, IAnimated
{
    private const float MinRotation = -0.05f;
    private const float MaxRotation = 0.05f;
    private float _rotatePerFrame = 0.001f;
    public Tree()
    {
        DrawOrigin = new Vector2(1f,2f);
        DrawSize = new Vector2(4f,8f);
        Anchor = new Vector2(0.5f, 1f);
        MapSize = new Point(2,2);
        Height = 3;
        TextureName = "Textures/Structures/Trees/tree";
    }

    public void Animate()
    {
        Rotation += _rotatePerFrame;
        if (Rotation >= MaxRotation || Rotation <= MinRotation)
            _rotatePerFrame = -_rotatePerFrame;
    }
}