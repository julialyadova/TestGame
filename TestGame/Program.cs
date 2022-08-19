using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TestGame;
using TestGame.Adapters;
using TestGame.Commands;
using TestGame.Core.Map;
using TestGame.Network;
using TestGame.UI;
using TestGame.UserInput;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices(
    services =>
    {
        services.AddHostedService<Game1>();
        
        services.AddScoped<Server>();
        services.AddScoped<Client>();
        services.AddScoped<NetworkSyncService>();

        services.AddScoped<GameUI>();
        services.AddScoped<WorldMap>();
        services.AddScoped<PlayerController>();
        
        services.AddScoped<FontsRepository>();
        services.AddScoped<UITexturesRepository>();
        services.AddScoped<MapTexturesRepository>();
        services.AddScoped<MapToScreenAdapter>();
        services.AddScoped<MapDrawer>();
        
        services.AddScoped<MoveInput, WASDMoveInput>();
        services.AddScoped<ZoomInput, MouseWheelZoomInput>();
        services.AddScoped<PointerInput,MousePointerInput>();
        
        services.AddScoped<UIInputAdapter>();
        services.AddScoped<MapInputAdapter>();
        services.AddScoped<PlayerInputAdapter>();
        
        services.AddScoped<StartServerCommand>();
        services.AddScoped<JoinGameCommand>();
        services.AddScoped<ExitPartyCommand>();

        services.AddSingleton(Config.FromFile("config.json"));
    });

var host = builder.Build();
host.Run();
