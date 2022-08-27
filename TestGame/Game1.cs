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
using TestGame.States;

namespace TestGame;

public class Game1 : Game, IHostedService
{

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private IServiceProvider _services;
    public Game1(IServiceProvider services)
    {
        _services = services;
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        ;
        var appLifetime = services.GetRequiredService<IHostApplicationLifetime>();
        Exiting += (sender, args) => appLifetime.StopApplication();
    }

    protected override void Initialize()
    {
        // _graphics.PreferredBackBufferWidth = _config.ScreenWidth;
        //_graphics.PreferredBackBufferHeight = _config.ScreenHeight;
        _graphics.ApplyChanges();


        base.Initialize();
    }

    protected override void LoadContent()
    {
        MyraEnvironment.Game = this;
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        ContentRepository.GraphicsDevice = GraphicsDevice;
        ContentRepository.ContentManager = Content;
        GameDrawer.SpriteBatch = _spriteBatch;
        GameState.LoadContent(_services);
    }

    protected override void Update(GameTime gameTime)
    {
        GameState.CurrentState.HandleInputs((float)gameTime.ElapsedGameTime.TotalSeconds);
        GameState.CurrentState.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        
        GameState.CurrentState.Draw();
        GameState.CurrentState.DrawUI();

        
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