using System.IO;
using Myra.Graphics2D.UI;
using TestGame.Drawing;
using TestGame.UI.Abstractions;

namespace TestGame.UI.Implementations;

public class MyraLoadingUI : GameDrawer, ILoadingUI
{
    private Desktop _desktop;
    private Label _messageLabel;
    private StringAnimation _loadingAnimation;
    
    public MyraLoadingUI()
    {
        Project project;
        using (var reader = new StreamReader("Content/Layouts/Loading.xmmp"))
        {
            var data = reader.ReadToEnd();
            project = Project.LoadFromXml(data);
        }
        
        _desktop = new Desktop();
        _desktop.Root = project.Root;
        
        _messageLabel = _desktop.Root.FindWidgetById("message") as Label;
        
        var loadingLabel = _desktop.Root.FindWidgetById("loading") as Label;
        _loadingAnimation = new StringAnimation
        (
            new[] { "loading.", "loading..", "loading..." },
            (str) => loadingLabel.Text = str, 
            1
        );
    }
    
    public void ShowMessage(string text)
    {
        _messageLabel.Text = text;
    }

    public void Update(float deltaTime)
    {
        _loadingAnimation.Update(deltaTime);
    }

    public void Draw()
    {
        _desktop.Render();
    }
}