using System.Diagnostics;
using TestGame.UI;

namespace TestGame.Commands;

public class LogCommand : ICommand
{
    private GameUI _ui;
    private string _message;
    
    public LogCommand(GameUI ui)
    {
        _ui = ui;
    }

    public LogCommand SetMessage(string message)
    {
        _message = message;
        return this;
    }

    public void Execute()
    {
        _ui.ShowMessage(_message);
        Debug.WriteLine(_message);
    }
}