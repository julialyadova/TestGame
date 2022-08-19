using TestGame.Network;
using TestGame.UI;

namespace TestGame.Commands;

public class HostGameCommand : ICommand
{
    private Server _server;

    public HostGameCommand(Server server)
    {
        _server = server;
    }
    
    public void Execute()
    {
        _server.Start();
    }
}