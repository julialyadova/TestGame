using TestGame.Network;

namespace TestGame.Commands;

public class JoinGameCommand : ICommand
{
    private Client _client;
    
    public JoinGameCommand(Client client)
    {
        _client = client;
    }
    
    public void Execute()
    {
        _client.Start();
    }
}