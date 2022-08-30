using TestGame.Core.Entities.Base;
using TestGame.Extensions;

namespace TestGame.Core.Entities.Structures;

public class Wall : Structure
{
    public Wall Left;
    public Wall Right;
    public Wall Top;
    public Wall Bottom;
    
    public Wall(int height)
    {
        Height = height;
        TextureName = "Textures/Structures/brick_wall";
    }

    public void Connect(Wall neighbour)
    {
        if (neighbour.Position.ToPoint() == Position.ToPoint().RightNeighbour())
        {
            Right = neighbour;
            neighbour.Left = this;
        }
        else if (neighbour.Position.ToPoint() == Position.ToPoint().LeftNeighbour())
        {
            Left = neighbour;
            neighbour.Right = this; 
        }
        else if (neighbour.Position.ToPoint() == Position.ToPoint().TopNeighbour())
        {
            Top = neighbour;
            neighbour.Bottom = this; 
        }
        else if (neighbour.Position.ToPoint() == Position.ToPoint().BottomNeighbour())
        {
            Bottom = neighbour;
            neighbour.Top = this; 
        }
    }

    public override void OnDestroy()
    {
        if (Left != null)
            Left.Right = null;
        if (Right != null)
            Right.Left = null;
        if (Top != null)
            Top.Bottom = null;
        if (Bottom != null)
            Bottom.Top = null;
        
        base.OnDestroy();
    }
}