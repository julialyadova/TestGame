using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using TestGame.Core.Entities.Base;
using TestGame.Core.Entities.Structures;
using TestGame.Extensions;

namespace TestGame.Core.Map;

public class WorldMap
{
    public int Seed;
    public readonly Point Size = new(100, 100);
    public readonly Rectangle Bounds;
    public Point SpawnPoint;
    public List<Structure>[] Structures;
    public Surface[,] SurfacesMap;
    public Point Pointer = Point.Zero;
    public Action<Structure> OnStructureRemoved;

    private Structure[,] _structuresMap;
    private readonly Point[] _neighbours = { new(-1, 0), new(1, 0), new(0, 1), new(1, 0) };

    public WorldMap()
    {
        Structures = new List<Structure>[Size.Y];
        _structuresMap = new Structure[Size.X,Size.Y];
        SurfacesMap = new Surface[Size.X, Size.Y];
        SpawnPoint = Size.Divide(2);
        Bounds = new Rectangle(Point.Zero, Size);
    }

    public void Load(int seed)
    {
        Structures = new List<Structure>[Size.Y];
        _structuresMap = new Structure[Size.X,Size.Y];
        SurfacesMap = new Surface[Size.X, Size.Y];
        new MapGenerator().Generate(this, seed);
    }
    
    public bool CanWalkTrough(Point position)
    {
        if (position.X <= 0 || position.X >= Size.X || position.Y <= 0 || position.Y >= Size.Y)
            return false;
        
        return _structuresMap[position.X, position.Y] == null || _structuresMap[position.X, position.Y].CanWalkThrough;
    }

    public void Build(Structure structure, Point position)
    {
        if (position.X < 0 || position.Y < 0 
            || position.X + structure.Size.X >= Size.X 
            || position.Y + structure.Size.Y >= Size.Y)
            return;
        
        for (int x = 0; x < structure.Size.X ; x++)
        for (int y = 0; y < structure.Size.Y; y++)
        {
            if (_structuresMap[position.X + x, position.Y + y] != null )
                return;
        }

        Structures[position.Y] ??= new List<Structure>();
        Structures[position.Y].Add(structure);
        structure.Position = position;


        for (int x = 0; x < structure.Size.X ; x++)
        for (int y = 0; y < structure.Size.Y; y++)
        {
            _structuresMap[position.X + x, position.Y + y] = structure;
        }

        if (structure is Wall wall)
        {
            ConnectNeighbourWalls(wall);
        }
        
    }

    public Structure GetStructureAt(Point position)
    {
        if (Bounds.Contains(position))
            return _structuresMap[position.X, position.Y];
        else
            return null;
    }

    public void RemoveStructure(int x, int y)
    {
        Remove(_structuresMap[x,y]);
    }

    public void Remove(Structure structure)
    {
        if (structure == null)
        {
            Debug.WriteLine("Game: Attempt to remove null structure");
            return;
        }
        
        structure.OnDestroy();
        
        Structures[structure.Position.Y].Remove(structure);
        for (int x = 0; x < structure.Size.X ; x++)
        for (int y = 0; y < structure.Size.Y; y++)
        {
            _structuresMap[structure.Position.X + x, structure.Position.Y + y] = null;
        }
        
        OnStructureRemoved?.Invoke(structure);
    }

    private void ConnectNeighbourWalls(Wall wall)
    {
        foreach (var point in _neighbours)
        {
            if (_structuresMap[wall.Position.X + point.X, wall.Position.Y + point.Y] is Wall neighbour)
                wall.Connect(neighbour);
        }
    }
}