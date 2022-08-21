using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myra;
using TestGame.Adapters;
using TestGame.Core.Map;
using TestGame.Network;
using TestGame.UI;
using TestGame.UserInput;

namespace TestGame;

public class Game1 : Game, IHostedService
{

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private WorldMap _map;
    private Config _config;
    private Server _server;
    private Client _client;
    private GameUI _gameUi;
    
    private readonly MapTexturesRepository _mapTexturesRepository;
    private readonly FontsRepository _fontsRepository;
    private readonly UITexturesRepository _uiTexturesRepository;
    
    private readonly PointerInput _pointerInput;
    private readonly ZoomInput _zoomInput;
    private readonly MoveInput _moveInput;
    private readonly ControlsInput _controlsInput;
    
    private readonly MapDrawer _mapDrawer;

    public Game1(IServiceProvider services)
    {
        _config = services.GetRequiredService<Config>();
        _server = services.GetRequiredService<Server>();
        _client = services.GetRequiredService<Client>();
        _gameUi = services.GetRequiredService<GameUI>();
        _map = services.GetRequiredService<WorldMap>();
        
        _mapTexturesRepository = services.GetRequiredService<MapTexturesRepository>();
        _uiTexturesRepository = services.GetRequiredService<UITexturesRepository>();
        _fontsRepository = services.GetRequiredService<FontsRepository>();
        
        _mapDrawer = services.GetRequiredService<MapDrawer>();

        //subscribe to inputs
        _pointerInput = services.GetRequiredService<PointerInput>();
        _zoomInput = services.GetRequiredService<ZoomInput>();
        _moveInput = services.GetRequiredService<MoveInput>();
        _controlsInput = services.GetRequiredService<ControlsInput>();

        var mapInputAdapter = services.GetRequiredService<MapInputAdapter>();
        _pointerInput.AddOnClickListener(mapInputAdapter.Click);
        _zoomInput.AddOnZoomListener(mapInputAdapter.Zoom);
        
        var playerInputAdapter = services.GetRequiredService<PlayerInputAdapter>();
        _moveInput.AddOnMoveListener(playerInputAdapter.Move);
        _controlsInput.AddOnControlPressedListener(playerInputAdapter.OnControlPressed);

        
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        var appLifetime = services.GetRequiredService<IHostApplicationLifetime>();
        Exiting += (sender, args) => appLifetime.StopApplication();
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = _config.ScreenWidth;
        _graphics.PreferredBackBufferHeight = _config.ScreenHeight;
        _graphics.ApplyChanges();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        MyraEnvironment.Game = this;
        _gameUi.LoadContent();
        
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
        _controlsInput.Update(gameTime);
        
        _map.Update((float) gameTime.ElapsedGameTime.TotalSeconds);

        _client.Update();
        _server.Update();
        

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        
        _spriteBatch.Begin();
        _mapDrawer.Draw(_spriteBatch);
        _spriteBatch.End();
        
        _gameUi.Draw();


        base.Draw(gameTime);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.Run(Run, cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _client.Disconnect();
        _server.Stop();
        return Task.Run(Exit, cancellationToken);
    }
}