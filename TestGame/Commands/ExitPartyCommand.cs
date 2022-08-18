using Microsoft.Xna.Framework;
using TestGame.Network;

namespace TestGame.Commands;

public class ExitPartyCommand : ICommand
{
    private Server _server;
    private Client _client;
    
    public ExitPartyCommand(Server server, Client client)
    {
        _server = server;
        _client = client;
    }
    public void Execute()
    {
        _server.Stop();
        _client.Stop();
    }
}