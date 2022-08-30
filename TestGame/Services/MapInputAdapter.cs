using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using TestGame.Core;
using TestGame.Core.Map;
using TestGame.Drawing;

namespace TestGame.Services;

public class MapInputAdapter
{
    private const int ZoomSpeedDivider = 50;

    private readonly ScreenAdapter _screenAdapter;
    private readonly World _world;

    public MapInputAdapter(IServiceProvider services)
    {
        _world = services.GetRequiredService<World>();
        _screenAdapter = services.GetRequiredService<ScreenAdapter>();
    }

    public void Zoom(int value)
    {
        //_screenAdapter.Zoom(value / ZoomSpeedDivider);
    }

    public bool Click(Point clickPosition)
    {
        //_world.Click(_screenAdapter.GetMapPosition(clickPosition));
        return true;
    }
}