using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TestGame.Adapters;
using TestGame.Commands;
using TestGame.Core.Entities.Creatures;
using TestGame.Network;
using TestGame.UI;
using TestGame.UserInput;

namespace TestGame;

public class Game1 : Game, IHostedService
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Player _player;
    private GameUI _ui;
    private readonly MapTexturesRepository _mapTexturesRepository;
    private readonly FontsRepository _fontsRepository;
    private readonly UITexturesRepository _uiTexturesRepository;
    private readonly MapToScreenAdapter _screenAdapter;
    private readonly MapDrawer _mapDrawer;
    private readonly PointerInput _pointerInput;
    private readonly ZoomInput _zoomInput;
    private readonly MoveInput _moveInput;

    private Server _server;
    private Client _client;

    public Game1(IServiceProvider services)
    {
        _mapTexturesRepository = services.GetRequiredService<MapTexturesRepository>();
        _fontsRepository = services.GetRequiredService<FontsRepository>();
        _uiTexturesRepository = services.GetRequiredService<UITexturesRepository>();
        _screenAdapter = services.GetRequiredService<MapToScreenAdapter>();
        _mapDrawer = services.GetRequiredService<MapDrawer>();
        
        _player = services.GetRequiredService<Player>();
        _ui = services.GetRequiredService<GameUI>();
        _server = services.GetRequiredService<Server>();
        _client = services.GetRequiredService<Client>();
        
        var mapInputAdapter = services.GetRequiredService<MapInputAdapter>();
        var uiInputAdapter = services.GetRequiredService<UIInputAdapter>();
        var playerInputAdapter = services.GetRequiredService<PlayerInputAdapter>();
        
        _pointerInput = services.GetRequiredService<PointerInput>();
        _pointerInput.AddOnClickListener(uiInputAdapter.Click);
        _pointerInput.AddOnClickListener(mapInputAdapter.Click);

        _zoomInput = services.GetRequiredService<ZoomInput>();
        _zoomInput.AddOnZoomListener(mapInputAdapter.Zoom);
        
        _moveInput = services.GetRequiredService<MoveInput>();
        _moveInput.AddOnMoveListener(playerInputAdapter.Move);


        _player.Position = new Vector2(10, 10);
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _screenAdapter.SetCenter(new Point(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2));
        Debug.WriteLine("Game started");
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        
        _uiTexturesRepository.LoadContent(GraphicsDevice, Content);
        _mapTexturesRepository.LoadContent(GraphicsDevice, Content);
        _fontsRepository.LoadContent(GraphicsDevice, Content);
    }

    protected override void Update(GameTime gameTime)
    {
        _pointerInput.Update(gameTime);
        _zoomInput.Update(gameTime);
        _moveInput.Update(gameTime);

        _server.Update();
        _client.Update();
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        
        _spriteBatch.Begin();
        _mapDrawer.Draw(_spriteBatch);
        _ui.Draw(_spriteBatch);
        _spriteBatch.End();

        base.Draw(gameTime);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.Run(Run, cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.Run(Exit, cancellationToken);
    }
}