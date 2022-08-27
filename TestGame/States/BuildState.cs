using System;
using System.IO;
using Microsoft.Xna.Framework.Input;
using Myra.Graphics2D.UI;
using TestGame.Drawing;

namespace TestGame.States;

public class BuildState : PlayGameState
{
    private readonly Desktop _desktop;
    public BuildState(IServiceProvider services) : base(services)
    {
        Project project;
        using (StreamReader reader = new StreamReader("Content/Layouts/BuildUI.xmmp"))
        {
            string data = reader.ReadToEnd();
            project = Project.LoadFromXml(data);
        }
        _desktop = new Desktop();
        _desktop.Root = project.Root;
    }

    public override void HandleInputs(float deltaTime)
    {
        base.HandleInputs(deltaTime);
        
        if (Keyboard.GetState().IsKeyDown(Keys.N))
            SetState(ExploreWorldState);
    }

    public override void DrawUI()
    {
        _desktop.Render();
    }
}