using System;
using Microsoft.Xna.Framework;
using TestGame.Core.Entities.Structures;
using TestGame.Core.Entities.Surfaces;
using TestGame.Entities;

namespace TestGame.Core.Map;

public class MapGenerator
{
    public void Generate(WorldMap map, int seed = 40006000)
    {
        var random = new Random(seed);
        for (int y = 0; y < map.Size.Y; y++)
        {
            map.Structures[y] = null;
            for (int x = 0; x < map.Size.X; x++)
            {
                map.StructuresMap[x, y] = null;
                map.SurfacesMap[x, y] = new Grass();
                if (random.Next(0, 10) == 1)
                {
                    map.Build(new Tree(), new Point(x,y));
                }
            }
        }
    }
}