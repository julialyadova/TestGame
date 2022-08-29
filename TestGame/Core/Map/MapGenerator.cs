using System;
using Microsoft.Xna.Framework;
using TestGame.Core.Entities.Structures;

namespace TestGame.Core.Map;

public class MapGenerator
{
    public void Generate(WorldMap map, int seed = 40006000)
    {
        map.Seed = seed;
        var random = new Random(seed);
        var position = Point.Zero;

        for (int x = 0; x < map.Size.X; x++)
        for (int y = 0; y < map.Size.Y; y++)
        {
            position.X = x;
            position.Y = y;
            
            map.Terrain.SetSurface(SurfaceType.Grass, position);
            if (random.Next(0, 10) == 1)
            {
                if (random.Next(0, 40) == 1)
                {
                    map.Build(new HighTree(), position);
                }
                else
                {
                    map.Build(new Tree(), position);
                    map.Terrain.SetSurface(SurfaceType.Podzol, position);
                }
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