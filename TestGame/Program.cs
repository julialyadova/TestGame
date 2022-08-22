using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TestGame;
using TestGame.Adapters;
using TestGame.Commands;
using TestGame.Core.Map;
using TestGame.Drawing;
using TestGame.Drawing.Repositories;
using TestGame.Network;
using TestGame.Services;
using TestGame.UserInput;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices(
    services =>
    {
        services.AddHostedService<Game1>();

        services.AddScoped<Server>();
        services.AddScoped<Client>();
        services.AddScoped<ServerPacketManager>();
        services.AddScoped<ClientPacketManager>();

        services.AddScoped<GameUI>();

        services.AddScoped<WorldMap>();
        services.AddScoped<PlayerController>();

        services.AddScoped<FontsRepository>();
        services.AddScoped<UITexturesRepository>();
        services.AddScoped<MapTexturesRepository>();
        services.AddScoped<PlayerTexturesRepository>();
        services.AddScoped<MapToScreenAdapter>();
        services.AddScoped<MapDrawer>();

        services.AddScoped<MoveInput, WASDMoveInput>();
        services.AddScoped<ZoomInput, MouseWheelZoomInput>();
        services.AddScoped<PointerInput, MousePointerInput>();
        services.AddScoped<ControlsInput, KeyboardControlsInput>();
        
        services.AddScoped<MapInputAdapter>();
        services.AddScoped<PlayerInputAdapter>();

        services.AddScoped<HostGameCommand>();
        services.AddScoped<JoinGameCommand>();
        services.AddScoped<DisconnectCommand>();
        services.AddScoped<ExitGameCommand>();
        services.AddScoped<LogCommand>();

        services.AddSingleton(Config.FromFile("config.json"));
    });

var host = builder.Build();
host.RunAsync();
host.WaitForShutdown();

