using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using FontStashSharp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TestGame.Core;
using TestGame.Core.Entities.Base;
using TestGame.Core.Entities.Creatures;
using TestGame.Core.Entities.Structures;
using TestGame.Core.Map;
using TestGame.Core.Players;
using TestGame.Drawing.Repositories;
using TestGame.Extensions;

namespace TestGame.Drawing;

public class MapDrawer : GameDrawer
{
    private readonly Rectangle _margins = new Rectangle(-10, -6, 20, 10);
    private MapTexturesRepository _textures;
    private PlayerTexturesRepository _playerTextures;
    private FontsRepository _fonts;

    private Rectangle _viewport = Rectangle.Empty;

    public MapDrawer(IServiceProvider services)
    {
        _textures = services.GetRequiredService<MapTexturesRepository>();
        _playerTextures = services.GetRequiredService<PlayerTexturesRepository>();
        _fonts = services.GetRequiredService<FontsRepository>();
    }

    public void Draw(World world, Camera camera)
    {
        SetMapViewport(camera);
        
        SpriteBatch.Begin(
            SpriteSortMode.Deferred,
            BlendState.AlphaBlend,
            null,
            null,
            null,
            null,
            camera.GetTransformMatrix(SpriteBatch.GraphicsDevice));

        DrawTerrain(world.Map.Terrain);
        DrawEntities(world);
        
        SpriteBatch.End();
    }

    private void SetMapViewport(Camera camera)
    {
        var screenViewport = camera.GetViewport(SpriteBatch.GraphicsDevice);
        _viewport = ScreenAdapter.GetMapRect(screenViewport);
        _viewport.X += _margins.X;
        _viewport.Y += _margins.Y;
        _viewport.Width += _margins.Width;
        _viewport.Height += _margins.Height;
    }

    private void DrawTerrain(Terrain terrain)
    {
        for (int mapX = _viewport.Left; mapX < _viewport.Right; mapX++)
        for (int mapY = _viewport.Top; mapY < _viewport.Bottom; mapY++)
        {
            var surface = terrain.GetSurfaceAt(new Point(mapX,mapY));
            if (surface == null)
                continue;
            Draw(_textures.GetTexture(surface.TextureName), new Rectangle(mapX, mapY, 1, 1));
        }
    }

    private void DrawEntities(World world)
    {
        var processedEntities = new HashSet<Entity>();
        for (int mapY = _viewport.Top; mapY < _viewport.Bottom; mapY++)
        {
            for (int mapX = _viewport.Left; mapX < _viewport.Right; mapX++)
            {
                var structure = world.Map.GetStructureAt(new Point(mapX, mapY));
                if (structure == null || processedEntities.Contains(structure))
                    continue;

                DrawEntity(structure);
                processedEntities.Add(structure);
            }
            DrawPlayersAtY(world.Players, mapY);
        }
    }
    

    private void DrawPlayersAtY(GamePlayers players, int y)
    {
        foreach (var player in players
                     .Where(p => (int)p.Position.Y == y)
                     .Where(p => p.Position.X > _viewport.Left && p.Position.X < _viewport.Right)
                     .OrderBy(p => p.Position.Y))
        {
            DrawPlayer(player);
        }
    }

    private void DrawPlayer(Player player)
    {
        var playerTexture = _playerTextures.GetTexture(player.TextureName);
        
        if (player.LooksLeft())
            DrawEntity(player, playerTexture.Side);
        else if (player.LooksRight())
            DrawEntity(player, playerTexture.Side);
        else if (player.LooksForward())
            DrawEntity(player, playerTexture.Front);
        else if (player.LooksBack())
            DrawEntity(player, playerTexture.Back);

        DrawPlayerName(player);
    }

    private void DrawPlayerName(Player player)
    {
        DrawString($"{player.Name}\nx:{player.Position.X:0.0}\ny:{player.Position.Y:0.0}", player.Position + player.DrawOrigin);
    }

    private void DrawEntity(Entity entity, Texture2D texture = null)
    {
        texture ??= _textures.GetTexture(entity.TextureName);
        
        
        var drawRect = new Rectangle
        (
            ScreenAdapter.GetScreenVector(entity.Position + entity.DrawOrigin).ToPoint(),
            ScreenAdapter.GetScreenVector(entity.DrawSize).ToPoint()
        );
        SpriteBatch.Draw(texture, drawRect, Color.White);
    }

    private void Draw(Texture2D texture, Rectangle mapRect)
    {
        SpriteBatch.Draw(texture, ScreenAdapter.GetScreenRect(mapRect), Color.White);
    }

    private void DrawString(string text, Vector2 mapPosition)
    {
        SpriteBatch.DrawString(_fonts.MainFont, text, ScreenAdapter.GetScreenVector(mapPosition), Color.White);
    }
}