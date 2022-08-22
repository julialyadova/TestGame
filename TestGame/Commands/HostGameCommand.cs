using TestGame.Network;

namespace TestGame.Commands;

public class HostGameCommand : ICommand
{
    private Server _server;
    private Client _client;
    private Config _config;
    
    public HostGameCommand(Server server, Client client, Config config)
    {
        _server = server;
        _client = client;
        _config = config;
    }
    
    public void Execute()
    {
        _server.Start(_config.ServerPort);
        _client.Connect("localhost", _config.ServerPort, _config.ConnectionKey, _config.Username);
    }
}