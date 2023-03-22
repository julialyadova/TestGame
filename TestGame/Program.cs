using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TestGame;
using TestGame.Core;
using TestGame.Core.Players;
using TestGame.Drawing;
using TestGame.Drawing.Repositories;
using TestGame.Input;
using TestGame.Network;
using TestGame.UI.Abstractions;
using TestGame.UI.Implementations;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices(
    services =>
    {
        services.AddHostedService<MainGame>();

        /* Network services */
        services.AddScoped<NetworkServiceProvider>();
        services.AddScoped<ServerPacketManager>();
        services.AddScoped<ClientPacketManager>();

        /* Main game classes */
        services.AddScoped<World>();
        services.AddScoped<Camera>();
        services.AddScoped<PlayerController>();

        /* Repositories */
        services.AddScoped<FontsRepository>();
        services.AddScoped<UITexturesRepository>();
        services.AddScoped<MapTexturesRepository>();
        services.AddScoped<PlayerTexturesRepository>();
        
        /* Game Drawers */
        services.AddScoped<MapDrawer>();

        /* User inputs */
        services.AddScoped<InputService>();

        /* UI */
        services.AddScoped<ILoadingUI, MyraLoadingUI>();
        services.AddScoped<IBuildUI, MyraBuildUI>();

        /* Config */
        services.AddSingleton(Config.FromFile("config.json"));
    });

var host = builder.Build();
host.Run();
//host.WaitForShutdown();

