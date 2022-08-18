using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TestGame.Core.Entities.Base;
using TestGame.Core.Entities.Structures;
using TestGame.Core.Map;
using TestGame.Core.Tools;
using TestGame.Tools;

namespace TestGame.Entities;

public class WorldMap
{
    public readonly Point Size = new(40, 40);
    public List<Structure>[] Structures;
    public Structure[,] StructuresMap;
    public Surface[,] SurfacesMap;
    public Point Pointer = Point.Zero;

    private Point[] _neighbours = new Point[4] { new Point(-1, 0), new Point(1, 0), new Point(0, 1), new Point(1, 0) };

    public WorldMap()
    {
        Structures = new List<Structure>[Size.Y];
        StructuresMap = new Structure[Size.X,Size.Y];
        SurfacesMap = new Surface[Size.X, Size.Y];
        Generate();
    }

    public void Generate()
    {
        new MapGenerator().Generate(this);
    }

    public void Hover(Point position)
    {
        Pointer = position;
    }

    public void Click(Point position)
    {
        //Build(new Wall(1),x,y);
        Build(new Tree(), position);
    }

    public bool TileIsEmpty(Point position)
    {
        if (position.X < 0 || position.X >= Size.X || position.Y < 0 || position.Y >= Size.Y)
            return false;
        
        return StructuresMap[position.X, position.Y] == null;
    }

    public void Build(Structure structure, Point position)
    {
        if (position.X + structure.Size.X >= Size.X || position.Y + structure.Size.Y >= Size.Y)
            return;
        
        for (int mapX = position.X; mapX < position.X + structure.Size.X; mapX++)
        for (int mapY = position.Y; mapY <  position.Y + structure.Size.Y; mapY++)
        {
            if (StructuresMap[mapX, mapY] != null)
                return;
        }
        
        if (Structures[ position.Y] == null)
            Structures[ position.Y] = new List<Structure>();
        Structures[ position.Y].Add(structure);

        structure.Position = position;
        for (int mapX = position.X; mapX < position.X + structure.Size.X; mapX++)
        for (int mapY =  position.Y; mapY <  position.Y + structure.Size.Y; mapY++)
        {
            StructuresMap[mapX, mapY] = structure;
        }

        if (structure is Wall wall)
        {
            ConnectNeighbourWalls(wall);
        }
    }

    private void ConnectNeighbourWalls(Wall wall)
    {
        foreach (var neighbour in _neighbours)
        {
            if (StructuresMap[wall.Position.X + neighbour.X, wall.Position.Y + neighbour.Y] is Wall other)
            {
                other.Connect(wall);
            }
        }
    }
}