using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using TestGame.Core.Map;

namespace TestGame.Adapters;

public class MapInputAdapter
{
    private const int ZoomSpeedDivider = 50;

    private MapToScreenAdapter _screenAdapter;
    private WorldMap _map;

    public MapInputAdapter(IServiceProvider services)
    {
        _map = services.GetRequiredService<WorldMap>();
        _screenAdapter = services.GetRequiredService<MapToScreenAdapter>();
    }

    public void Zoom(int value)
    {
        _screenAdapter.Zoom(value / ZoomSpeedDivider);
    }

    public void Hover(Point pointerPosition)
    {
        _map.Hover(_screenAdapter.GetMapPosition(pointerPosition));
    }

    public bool Click(Point clickPosition)
    {
        _map.Click(_screenAdapter.GetMapPosition(clickPosition));
        return true;
    }
}