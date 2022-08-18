using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TestGame.Adapters;
using TestGame.Commands;
using TestGame.Core.Entities.Creatures;
using TestGame.Entities;
using TestGame.Tools;
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
    private readonly MapInputAdapter _mapInputAdapter;
    private readonly MapToScreenAdapter _screenAdapter;
    private readonly PlayerInputAdapter _playerInputAdapter;
    private readonly MapDrawer _mapDrawer;

    public Game1(IServiceProvider services)
    {
        _mapTexturesRepository = services.GetRequiredService<MapTexturesRepository>();
        _mapInputAdapter = services.GetRequiredService<MapInputAdapter>();
        _screenAdapter = services.GetRequiredService<MapToScreenAdapter>();
        _playerInputAdapter = services.GetRequiredService<PlayerInputAdapter>();
        _mapDrawer = services.GetRequiredService<MapDrawer>();
        _player = services.GetRequiredService<Player>();
        _player.Position = new Vector2(10, 10);
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _ui = new GameUI(_graphics);
        var button = new Button(new Rectangle(10, 10, 100, 60));
        button.SetCommand(new SelectBuildingToolCommand(_player));
        _ui.Add(button);
        
        _screenAdapter.SetCenter(new Point(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2));
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        
        _ui.LoadContent(Content);
        _mapTexturesRepository.LoadContent(GraphicsDevice, Content);
    }

    protected override void Update(GameTime gameTime)
    {
        _mapInputAdapter.Update(gameTime);
        _playerInputAdapter.Update(gameTime);

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