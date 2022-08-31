using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TestGame;
using TestGame.Core;
using TestGame.Core.Players;
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

        services.AddScoped<NetworkServiceProvider>();
        services.AddScoped<ServerPacketManager>();
        services.AddScoped<ClientPacketManager>();

        services.AddScoped<World>();
        services.AddScoped<PlayerController>();

        services.AddScoped<FontsRepository>();
        services.AddScoped<UITexturesRepository>();
        services.AddScoped<MapTexturesRepository>();
        services.AddScoped<PlayerTexturesRepository>();
        services.AddScoped<MapDrawer>();

        services.AddScoped<IMoveInput, WASDMoveInput>();
        services.AddScoped<IZoomInput, MouseWheelZoomInput>();
        services.AddScoped<IPointerInput, MousePointerInput>();
        services.AddScoped<IControlsInput, KeyboardControlsInput>();
        services.AddScoped<KeyboardInput>();

        
        services.AddScoped<MapInputAdapter>();
        services.AddScoped<PlayerInputAdapter>();

        services.AddSingleton(Config.FromFile("config.json"));
    });

var host = builder.Build();
host.Run();
//host.WaitForShutdown();

