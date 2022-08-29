using System;
using Microsoft.Xna.Framework;
using TestGame.Core.Entities.Base;

namespace TestGame.Core.Map;

public class Terrain
{
    private readonly Surface _grass = new Surface("Textures/Terrain/grass");
    private readonly Surface _podzol = new Surface("Textures/Terrain/podzol");
    private Surface[,] _map;

    public Terrain(Point size)
    {
        _map = new Surface[size.X, size.Y];
    }

    public void SetSurface(SurfaceType surfaceType, Point position)
    {
        if (surfaceType == SurfaceType.Grass)
            _map[position.X, position.Y] = _grass;
        else if (surfaceType == SurfaceType.Podzol)
            _map[position.X, position.Y] = _podzol;
        else
            throw new NotImplementedException($"Surface of type {surfaceType} doesn't exist in Terrain class");
    }

    public Surface GetSurfaceAt(Point position)
    {
        return _map[position.X, position.Y];
    }
    
    public Surface GetSurfaceAt(Vector2 position)
    {
        return _map[(int)position.X, (int)position.Y];
    }
}
