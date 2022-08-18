using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Xna.Framework;
using TestGame;
using TestGame.Adapters;
using TestGame.Core.Entities.Creatures;
using TestGame.Entities;
using TestGame.Tools;
using TestGame.UserInput;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices(
    services =>
    {
        services.AddHostedService<Game1>();

        services.AddScoped<WorldMap>();
        services.AddScoped<Player>();
        
        services.AddScoped<MapTexturesRepository>();
        services.AddScoped<MapToScreenAdapter>();
        services.AddScoped<MapDrawer>();
        
        services.AddScoped<IMoveInput, WASDMoveInput>();
        services.AddScoped<IZoomInput, MouseWheelZoomInput>();
        services.AddScoped<IPointerInput,MouseLBInput>();
        
        services.AddScoped<MapInputAdapter>();
        services.AddScoped<PlayerInputAdapter>();
    });

var host = builder.Build();
host.Run();
