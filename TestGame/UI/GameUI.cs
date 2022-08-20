using System;
using System.IO;
using Microsoft.Xna.Framework;
using Myra;
using Myra.Graphics2D.UI;


namespace TestGame.UI;

public class GameUI
{
    private Desktop _desktop;
    private Project _project;
    private Project _joinGameDialog;

    public void LoadContent(Game game)
    {
        MyraEnvironment.Game = game;
        LoadProject();
        _desktop = new Desktop();
        _desktop.Root = _project.Root;
        var settings = _desktop.Root.FindWidgetById("settings") as TextButton;
        settings.Click += (sender, args) =>
        {
            (_joinGameDialog.Root as Dialog).ShowModal(_desktop);
        };
    }

    private void LoadProject()
    {
        using (StreamReader reader = new StreamReader("Content/Layouts/layout.xmmp"))
        {
            string data = reader.ReadToEnd();
            _project = Project.LoadFromXml(data);
        }
        using (StreamReader reader = new StreamReader("Content/Layouts/menu.xmmp"))
        {
            string data = reader.ReadToEnd();
            _joinGameDialog = Project.LoadFromXml(data);
        }
    }
    
    public void Draw()
    {
        _desktop.Render();
    }
}