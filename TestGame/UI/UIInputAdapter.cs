using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using TestGame.UI;
using TestGame.UserInput;

namespace TestGame.Adapters;

public class UIInputAdapter
{
    private GameUI _ui;

    public UIInputAdapter(IServiceProvider services)
    {
        _ui = services.GetRequiredService<GameUI>();
    }

    public bool Click(Point clickPosition)
    {
        return _ui.CheckClick(clickPosition);
    }
}