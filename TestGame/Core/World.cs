using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using TestGame.Core.Entities.Structures;
using TestGame.Core.Map;
using TestGame.Core.Players;

namespace TestGame.Core;

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

    public async Task LoadAsync(Save save)
    {
        IsLoaded = false;
        await Task.Run(() => Map.Load(save.MapSeed));
        IsLoaded = true;
    }
    
    public void Quit()
    {
        IsLoaded = false;
    }
}