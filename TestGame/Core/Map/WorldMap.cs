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
    public int Seed; //todo remove to generation info class
    public readonly Point Size;
    private Rectangle _bounds;
    public Point SpawnPoint; //todo remove to World Class
    public List<Structure>[] Structures;
    public Terrain Terrain;
    public Point Pointer = Point.Zero;
    public Action<Structure> OnStructureRemoved;

    private Structure[,] _structuresMap;
    private readonly Point[] _neighbours = { new(-1, 0), new(1, 0), new(0, 1), new(1, 0) };
    
    public WorldMap(Point size)
    {
        Size = size;
        _bounds = new Rectangle(Point.Zero, Size);

        Structures = new List<Structure>[Size.Y];
        _structuresMap = new Structure[Size.X,Size.Y];
        Terrain = new Terrain(Size);
        SpawnPoint = Size.Divide(2);
    }


    public void Clear()
    {
        Structures = new List<Structure>[Size.Y];
        _structuresMap = new Structure[Size.X,Size.Y];
        Terrain = new Terrain(Size);
    }
    
    public bool CanWalkTrough(Point position)
    {
        if (!_bounds.Contains(position))
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
        if (_bounds.Contains(position))
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