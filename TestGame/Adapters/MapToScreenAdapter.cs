using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TestGame.Extensions;

namespace TestGame.Adapters;

public class MapToScreenAdapter
{
    private const int DefaultTileSize = 32;
    private const int MinTileSize = 32;
    private const int MaxTileSize = 128;
    
    public int TileSize
    {
        get => _tileSize;
    }

    private int _tileSize = DefaultTileSize;
    private Point _offset;
    private Point _center;

    public void SetCenter(Point center)
    {
        _center = center;
    }

    public void SetMapOffset(Vector2 offset)
    {
        _offset = (offset * _tileSize).ToPoint() + _center;
    }

    public Point GetScreenPosition(Point mapPosition)
    {
        return mapPosition.Multiply(_tileSize) + _offset;
    }
    
    public Vector2 GetScreenPosition(Vector2 mapPosition)
    {
        return mapPosition * _tileSize + _offset.ToVector2();
    }

    public int GetScreenLength(int length)
    {
        return length * _tileSize;
    }

    public Point GetMapPosition(Point screenPosition)
    {
        return (screenPosition - _offset).Divide(_tileSize);
    }
    
    public Vector2 GetMapPosition(Vector2 screenPosition)
    {
        return (screenPosition - _offset.ToVector2()) / _tileSize;
    }

    public void Zoom(int value)
    {
        _tileSize = Math.Clamp(_tileSize+value, MinTileSize, MaxTileSize);
    }
}