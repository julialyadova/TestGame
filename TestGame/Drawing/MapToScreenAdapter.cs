using System;
using Microsoft.Xna.Framework;
using TestGame.Core;
using TestGame.Core.Map;
using TestGame.Extensions;

namespace TestGame.Drawing;

public class MapToScreenAdapter
{
    private const int DefaultTileSize = 50;
    private const int MinTileSize = 32;
    private const int MaxTileSize = 64;

    public int TileSize { get; private set; }
    public Vector2 MapOffset { get; private set; }

    public Point ScreenOffset { get; private set; }

    public Point ScreenSize { get; }
    private Point ScreenCenter { get; }
    public Rectangle MapViewport => _mapViewport;
    private Rectangle _mapViewport;
    private WorldMap _map;

    public MapToScreenAdapter(Config config, World world)
    {
        _map = world.Map;
        ScreenSize = new Point(config.ScreenWidth, config.ScreenHeight);
        ScreenCenter = new Point(config.ScreenWidth / 2, config.ScreenHeight / 2);
        TileSize = DefaultTileSize;
        
        _mapViewport.Width = ScreenSize.X / (TileSize) + 2;
        _mapViewport.Height = ScreenSize.Y / (TileSize) + 3;
        _mapViewport.X = 0;
        _mapViewport.Y = 0;
    }
    

    public void CenterMap(Vector2 playerPosition)
    {
        _mapViewport.Width = ScreenSize.X / (TileSize) + 2;
        _mapViewport.Height = ScreenSize.Y / (TileSize) + 3;
        _mapViewport.X = (int)playerPosition.X - _mapViewport.Width / 2;
        _mapViewport.Y = (int)playerPosition.Y - _mapViewport.Height / 2;
        if (_mapViewport.X < 0)
            _mapViewport.X = 0;
        if (_mapViewport.Y < 0)
            _mapViewport.Y = 0;
        if (_mapViewport.Right > _map.Size.X)
            _mapViewport.Width = _map.Size.X - _mapViewport.X;
        if (_mapViewport.Bottom > _map.Size.Y)
            _mapViewport.Height = _map.Size.Y - _mapViewport.Y;
        
        MapOffset = -playerPosition;
        ScreenOffset = (-playerPosition * TileSize).ToPoint() + ScreenCenter;
    }

    public Point GetScreenPosition(Point mapPosition)
    {
        return mapPosition.Multiply(TileSize) + ScreenOffset;
    }
    
    public Vector2 GetScreenPosition(Vector2 mapPosition)
    {
        return mapPosition * TileSize + ScreenOffset.ToVector2();
    }

    public int GetScreenLength(int length)
    {
        return length * TileSize;
    }
    
    public int GetScreenLength(float length)
    {
        return (int) (length * TileSize);
    }

    public Point GetMapPosition(Point screenPosition)
    {
        return (screenPosition - ScreenOffset).Divide(TileSize);
    }
    
    public Vector2 GetMapPosition(Vector2 screenPosition)
    {
        return (screenPosition - ScreenOffset.ToVector2()) / TileSize;
    }

    public void Zoom(int value)
    {
        TileSize = Math.Clamp(TileSize+value, MinTileSize, MaxTileSize);
        UpdateViewport();
    }

    private void UpdateViewport()
    {
        _mapViewport.X = -(int)MapOffset.X;
        _mapViewport.Y = -(int)MapOffset.Y;
        _mapViewport.Width = ScreenSize.X / TileSize;
        _mapViewport.Height = ScreenSize.Y / TileSize;
    }
}