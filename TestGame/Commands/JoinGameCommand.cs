using TestGame.Network;

namespace TestGame.Commands;

public class JoinGameCommand : ICommand
{
    private Client _client;
    private Config _config;

    public JoinGameCommand(Client client, Config config)
    {
        _client = client;
        _config = config;
    }
    
    public void Execute()
    {
        _client.Connect(_config.ServerHost, _config.ServerPort, _config.ConnectionKey, _config.Username);
    }
}