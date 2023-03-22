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
    public Point Size { get; }
    public Rectangle Bounds { get;}
    public Terrain Terrain;
    public Action<Structure> OnStructureRemoved;

    private Structure[,] _structuresMap;
    private List<Structure> _structures = new();
    private readonly Point[] _neighbours = { new(-1, 0), new(1, 0), new(0, 1), new(1, 0) };
    
    public WorldMap(Point size)
    {
        Size = size;
        Bounds = new Rectangle(Point.Zero, Size);
        _structuresMap = new Structure[Size.X,Size.Y];
        Terrain = new Terrain(Size);
    }


    public IEnumerable<Entity> GetEntities()
    {
        return _structures;
    }

    public void Clear()
    {
        _structuresMap = new Structure[Size.X,Size.Y];
        Terrain = new Terrain(Size);
    }
    
    public bool CanWalkTrough(Point position)
    {
        if (!Bounds.Contains(position))
            return false;
        
        return _structuresMap[position.X, position.Y] == null || _structuresMap[position.X, position.Y].CanWalkThrough;
    }

    public void Build(Structure structure, Point position)
    {
        if (position.X < 0 || position.Y < 0 
            || position.X + structure.MapSize.X >= Size.X 
            || position.Y + structure.MapSize.Y >= Size.Y)
            return;
        
        for (int x = 0; x < structure.MapSize.X ; x++)
        for (int y = 0; y < structure.MapSize.Y; y++)
        {
            if (_structuresMap[position.X + x, position.Y + y] != null )
                return;
        }
        
        _structures.Add(structure);
        structure.Position = position.ToVector2();


        for (int x = 0; x < structure.MapSize.X ; x++)
        for (int y = 0; y < structure.MapSize.Y; y++)
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
        
        for (int x = 0; x < structure.MapSize.X ; x++)
        for (int y = 0; y < structure.MapSize.Y; y++)
        {
            _structuresMap[(int)structure.Position.X + x, (int)structure.Position.Y + y] = null;
        }

        _structures.Remove(structure);
        OnStructureRemoved?.Invoke(structure);
    }

    private void ConnectNeighbourWalls(Wall wall)
    {
        foreach (var point in _neighbours)
        {
            if (_structuresMap[(int)wall.Position.X + point.X, (int)wall.Position.Y + point.Y] is Wall neighbour)
                wall.Connect(neighbour);
        }
    }
}