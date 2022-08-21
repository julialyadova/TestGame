using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using TestGame.Core.Entities.Base;
using TestGame.Core.Entities.Creatures;
using TestGame.Core.Entities.Structures;
using TestGame.Core.Players;
using TestGame.Extensions;

namespace TestGame.Core.Map;

public class WorldMap
{
    public bool Loaded;
    public int Seed;
    public readonly Point Size = new(100, 100);
    public Point SpawnPoint;
    public GamePlayers Players;
    public List<Structure>[] Structures;
    public Surface[,] SurfacesMap;
    public Point Pointer = Point.Zero;
    public Action<Structure> OnStructureRemoved;

    private Structure[,] _structuresMap;
    private Point[] _neighbours = new Point[] { new Point(-1, 0), new Point(1, 0), new Point(0, 1), new Point(1, 0) };

    public WorldMap(Config config)
    {
        Players = new GamePlayers();
        Structures = new List<Structure>[Size.Y];
        _structuresMap = new Structure[Size.X,Size.Y];
        SurfacesMap = new Surface[Size.X, Size.Y];
        SpawnPoint = Size.Divide(2);
    }

    public void Load(int seed)
    {
        Loaded = false;
        Players.Clear();
        Structures = new List<Structure>[Size.Y];
        _structuresMap = new Structure[Size.X,Size.Y];
        SurfacesMap = new Surface[Size.X, Size.Y];
        new MapGenerator().Generate(this, seed);
        Loaded = true;
    }
    
    public void UnLoad()
    {
        Loaded = false;
    }

    public void Update(float deltaTime)
    {
    }

    public void Hover(Point position)
    {
        Pointer = position;
    }

    public void Click(Point position)
    {
        //Build(new Wall(1),x,y);
        //Build(new Tree(), position);
    }

    public bool CanWalkTrough(Point position)
    {
        if (position.X <= 0 || position.X >= Size.X || position.Y <= 0 || position.Y >= Size.Y)
            return false;
        
        return _structuresMap[position.X, position.Y] == null || _structuresMap[position.X, position.Y].CanWalkThrough;
    }

    public void Build(Structure structure, Point position)
    {
        if (position.X + structure.Size.X >= Size.X || position.Y + structure.Size.Y >= Size.Y)
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
        return _structuresMap[position.X, position.Y];
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
        foreach (var neighbour in _neighbours)
        {
            if (_structuresMap[wall.Position.X + neighbour.X, wall.Position.Y + neighbour.Y] is Wall other)
            {
                other.Connect(wall);
            }
        }
    }
}