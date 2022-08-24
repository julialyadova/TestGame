using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using TestGame.Core.Entities.Structures;
using TestGame.Core.Players;

namespace TestGame.Core.Map;

public class World
{
    public bool IsLoaded;
    public readonly WorldMap Map;
    public readonly GamePlayers Players;

    public World()
    {
        Map = new ();
        Players = new();
    }
    
    public void Click(Point position)
    {
        if (IsLoaded)
            Map.Build(new Wall(1), position);
    }

    public void Load(Save save)
    {
        IsLoaded = false;
        Map.Load(save.MapSeed);
        IsLoaded = true;
    }
    
    public void Quit()
    {
        IsLoaded = false;
    }
}