using TestGame.Network;

namespace TestGame.Commands;

public class StartServerCommand : ICommand
{
    private Server _server;
    
    public StartServerCommand(Server server)
    {
        _server = server;
    }
    
    public void Execute()
    {
        _server.Start();
    }
}