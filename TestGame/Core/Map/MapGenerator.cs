using System;
using Microsoft.Xna.Framework;
using TestGame.Core.Entities.Structures;
using TestGame.Core.Entities.Surfaces;
using TestGame.Entities;

namespace TestGame.Core.Map;

public class MapGenerator
{
    public void Generate(WorldMap map)
    {
        var random = new Random();
        for (int x = 0; x < map.Size.X; x++)
        for (int y = 0; y < map.Size.Y; y++)
        {
            map.SurfacesMap[x, y] = new Grass();
            if (random.Next(0, 10) < 2)
            {
                if (random.Next(0,2) == 1)
                    map.Build(new Tree(), new Point(x,y));
                else
                    map.Build(new SmallTree(), new Point(x,y));
            }
            
            if (random.Next(0,10) == 5)
                map.SurfacesMap[x, y] = new Water();
        }
    }
}