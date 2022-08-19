using Microsoft.Xna.Framework;
using TestGame.Network;
using TestGame.UI;

namespace TestGame.Commands;

public class DisconnectCommand : ICommand
{
    private Server _server;
    private Client _client;

    public DisconnectCommand(Server server, Client client)
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