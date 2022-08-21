using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TestGame.Core.Entities.Base;
using TestGame.Core.Entities.Structures;
using TestGame.Core.Entities.Surfaces;

namespace TestGame.Core.Map;

public class MapGenerator
{
    public void Generate(WorldMap map, int seed = 40006000)
    {
        map.Seed = seed;
        var random = new Random(seed);

        for (int x = 0; x < map.Size.X; x++)
        for (int y = 0; y < map.Size.Y; y++)
        {
            map.SurfacesMap[x, y] = new Grass();
            if (random.Next(0, 10) == 1)
            {
                if (random.Next(0, 40) == 1)
                    map.Build(new HighTree(), new Point(x,y));
                else
                    map.Build(new Tree(), new Point(x,y));
            }

            if (random.Next(0, 50) == 1)
            {
                GenerateFarm(map,random,x,y);
            }
        }
        
    }

    private void GenerateFarm(WorldMap map, Random random, int x, int y)
    {
        var farmSize = random.Next(2, 6);
        {
            for (int farmX = x; farmX < x + farmSize; farmX++)
            for (int farmY = y; farmY < y + farmSize; farmY++)
            {
                map.Build(new Farm(), new Point(farmX, farmY));
            }
        }
    }
}