using System.IO;
using Myra.Graphics2D.UI;
using TestGame.UI.Abstractions;

namespace TestGame.UI.Implementations;

public class MyraBuildUI : IBuildUI
{
    private readonly Desktop _desktop;

    public MyraBuildUI()
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
    
    public void Draw()
    {
        _desktop.Render();
    }
}