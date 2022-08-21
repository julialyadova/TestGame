using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using TestGame.Core.Entities.Base;
using TestGame.Core.Entities.Creatures;
using TestGame.Core.Entities.Structures;
using TestGame.Core.Players;

namespace TestGame.Core.Map;

public class WorldMap
{
    public bool Loaded;
    public int Seed;
    public Point SpawnPoint = Point.Zero;
    public readonly Point Size = new(4000, 4000);
    public GamePlayers Players;
    public List<Structure>[] Structures;
    public Structure[,] StructuresMap;
    public Surface[,] SurfacesMap;
    public Point Pointer = Point.Zero;
    public Action<Structure> OnStructureRemoved;

    private Point[] _neighbours = new Point[] { new Point(-1, 0), new Point(1, 0), new Point(0, 1), new Point(1, 0) };

    public WorldMap(Config config)
    {
        Players = new GamePlayers();
        Structures = new List<Structure>[Size.Y];
        StructuresMap = new Structure[Size.X,Size.Y];
        SurfacesMap = new Surface[Size.X, Size.Y];
    }

    public void Load(int seed)
    {
        Loaded = false;
        Players.Clear();
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
        
        for (int x = 0; x < structure.Size.X ; x++)
        for (int y = 0; y < structure.Size.Y; y++)
        {
            if (StructuresMap[position.X + x, position.Y + y] != null)
                return;
        }

        Structures[position.Y] ??= new List<Structure>();
        Structures[position.Y].Add(structure);
        structure.Position = position;
        
        for (int x = 0; x < structure.Size.X ; x++)
        for (int y = 0; y < structure.Size.Y; y++)
        {
            StructuresMap[position.X + x, position.Y + y] = structure;
        }

        if (structure is Wall wall)
        {
            ConnectNeighbourWalls(wall);
        }
    }

    public void RemoveStructure(int x, int y)
    {
        Remove(StructuresMap[x,y]);
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
            StructuresMap[structure.Position.X + x, structure.Position.Y + y] = null;
        }
        
        OnStructureRemoved?.Invoke(structure);
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