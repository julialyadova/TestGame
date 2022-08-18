using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using TestGame.Entities;
using TestGame.UserInput;

namespace TestGame.Adapters;

public class MapInputAdapter
{
    private const int ZoomSpeedDivider = 50;

    private MapToScreenAdapter _screenAdapter;
    private IZoomInput _zoomInput;
    private IPointerInput _pointerInput;
    private WorldMap _map;

    public MapInputAdapter(IServiceProvider services)
    {
        _map = services.GetRequiredService<WorldMap>();
        _screenAdapter = services.GetRequiredService<MapToScreenAdapter>();
        _zoomInput = services.GetRequiredService<IZoomInput>();
        _pointerInput = services.GetRequiredService<IPointerInput>();
    }

    public void Update(GameTime gameTime)
    {
        _pointerInput.UpdateState();
        if (_pointerInput.GetState() == MouseInputState.Click && !_pointerInput.ClickTargetReached())
        {
            Click(_pointerInput.GetPosition());
            _pointerInput.EndClick();
        }
        else if (_pointerInput.GetState() == MouseInputState.Hover)
        {
            Hover(_pointerInput.GetPosition());
        }

        _zoomInput.UpdateState();
        Zoom(_zoomInput.GetZoomChangeValue());
    }

    public void Zoom(int value)
    {
        _screenAdapter.Zoom(value / ZoomSpeedDivider);
    }

    public void Hover(Point pointerPosition)
    {
        _map.Hover(_screenAdapter.GetMapPosition(pointerPosition));
    }

    public void Click(Point clickPosition)
    {
        _map.Click(_screenAdapter.GetMapPosition(clickPosition));
    }
}