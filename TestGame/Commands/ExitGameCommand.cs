using Microsoft.Extensions.Hosting;

namespace TestGame.Commands;

public class ExitGameCommand : ICommand
{
    private IHostApplicationLifetime _hostApplicationLifetime;
    
    public ExitGameCommand(IHostApplicationLifetime appLifetime)
    {
        _hostApplicationLifetime = appLifetime;
    }
    public void Execute()
    {
        _hostApplicationLifetime.StopApplication();
    }
}