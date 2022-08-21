using Microsoft.Xna.Framework;
using TestGame.Network;
using TestGame.UI;

namespace TestGame.Commands;

public class DisconnectCommand : ICommand
{
    private Client _client;

    public DisconnectCommand(Client client)
    {
        _client = client;
    }
    public void Execute()
    {
        _client.Disconnect();
    }
}