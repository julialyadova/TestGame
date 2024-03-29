﻿using System.Diagnostics;
using Microsoft.Xna.Framework;
using TestGame.Core.Entities.Creatures;
using TestGame.Core.Map;

namespace TestGame.Core.Players;

public class PlayerController
{
    private const int MoveSpeed = 8;
    
    public Player Player;
    public Point Focus;
    private Config _config;
    private WorldMap _map;
    
    public PlayerController(WorldMap map)
    {
        _map = map;
    }

    public void Move(Vector2 direction, float deltaTime)
    {
        if (direction == Vector2.Zero)
            return;
        
        var newPosition = Player.Position + direction * MoveSpeed * deltaTime;
        var tile = newPosition.ToPoint();
        if (_map.CanWalkTrough(tile))
            Player.Position = newPosition;

        Player.Direction = direction;
        Focus = Player.Position.ToPoint() + Player.Direction.ToPoint();
    }

    public void Interact()
    {
        Debug.WriteLine("Interact");
        var target = _map.GetStructureAt(Focus);
        if (target != null)
            _map.Remove(target);
    }
}