using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myra;
using TestGame.Drawing;
using TestGame.Drawing.Repositories;
using TestGame.States.Base;

namespace TestGame;

public class MainGame : Game, IHostedService
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private IServiceProvider _services;
    
    public MainGame(IServiceProvider services)
    {
        _services = services;
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.PreferredBackBufferHeight = 720;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        var appLifetime = services.GetRequiredService<IHostApplicationLifetime>();
        Exiting += (sender, args) => appLifetime.StopApplication();
    }

    protected override void Initialize()
    {
        _graphics.ApplyChanges();
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        
        MyraEnvironment.Game = this;
        ContentRepository.GraphicsDevice = GraphicsDevice;
        ContentRepository.ContentManager = Content;
        GameDrawer.SpriteBatch = _spriteBatch;
        MainState.LoadContent(_services);
        GameState.LoadContent(_services);
    }

    protected override void Update(GameTime gameTime)
    {
        MainState.CurrentState.HandleInputs((float)gameTime.ElapsedGameTime.TotalSeconds);
        MainState.CurrentState.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        
        MainState.CurrentState.Draw();

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