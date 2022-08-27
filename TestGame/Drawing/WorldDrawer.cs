using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TestGame.Core;
using TestGame.Core.Entities.Base;
using TestGame.Core.Entities.Creatures;
using TestGame.Core.Entities.Structures;
using TestGame.Core.Map;
using TestGame.Drawing.Repositories;
using TestGame.Extensions;

namespace TestGame.Drawing;

public class MapDrawer : GameDrawer
{
    private readonly int _textureSize = 128;
    private readonly int _textureSideWidth = 16;
    private readonly float _textureSidePart;
    private readonly int _maxStructSize = 10;
    private World _world;
    private WorldMap _map;
    private MapToScreenAdapter _screenAdapter;
    private MapTexturesRepository _textures;
    private PlayerTexturesRepository _playerTextures;
    private FontsRepository _fonts;
    
    private Rectangle _drawRect = Rectangle.Empty;
    private Rectangle _sourceRect = Rectangle.Empty;
    private Rectangle _viewport = Rectangle.Empty;

    public MapDrawer(IServiceProvider services)
    {
        _world = services.GetRequiredService<World>();
        _map = _world.Map;
        _screenAdapter = services.GetRequiredService<MapToScreenAdapter>();
        _textures = services.GetRequiredService<MapTexturesRepository>();
        _playerTextures = services.GetRequiredService<PlayerTexturesRepository>();
        _fonts = services.GetRequiredService<FontsRepository>();
        _textureSidePart = (float)_textureSideWidth / _textureSize;
    }

    public void Draw()
    {
        SpriteBatch.Begin();
        _viewport = _screenAdapter.MapViewport;

        DrawSurfaces(SpriteBatch);
        DrawStructures(SpriteBatch);
        SpriteBatch.End();
    }

    void DrawSurfaces(SpriteBatch spriteBatch)
    {
        _drawRect.Width = _screenAdapter.TileSize;
        _drawRect.Height = _screenAdapter.TileSize;
        
        for (int mapX = _viewport.Left; mapX < _viewport.Right; mapX++)
        for (int mapY = _viewport.Top; mapY < _viewport.Bottom; mapY++)
        {
            var surface = _map.SurfacesMap[mapX, mapY];
            _drawRect.Location = _screenAdapter.GetScreenPosition(new Point(mapX, mapY));
            spriteBatch.Draw(_textures.GetTexture(surface.TextureName), _drawRect, Color.White );
        }
    }

    private void DrawStructures(SpriteBatch spriteBatch)
    {
        for (int level = _viewport.Y; level < Math.Min(_viewport.Bottom + _maxStructSize, _map.Size.Y); level++)
        {
            if (_map.Structures[level] == null)
                continue;

            foreach (var structure in _map.Structures[level])
            {
                if (structure.Position.X < _viewport.X - _maxStructSize || structure.Position.X >= _viewport.Right)
                    continue;

                UpdateDrawRectForStructure(structure);
    
                if (structure is Wall wall)
                    DrawWall(wall, spriteBatch);
                else if (structure is Farm farm)
                    DrawFarm(farm, spriteBatch);
                else
                    spriteBatch.Draw(_textures.GetTexture(structure.TextureName), _drawRect, Color.White );
            }
            
            DrawPlayersAtY(level, spriteBatch);
        }
    }

    private void UpdateDrawRectForStructure(Structure structure)
    {
        _drawRect.Location = _screenAdapter.GetScreenPosition(new Point(
            structure.Position.X,
            structure.Position.Y - structure.Height));
        _drawRect.Height = _screenAdapter.GetScreenLength(structure.Size.Y + structure.Height);
        _drawRect.Width = _screenAdapter.GetScreenLength(structure.Size.X);
    }

    private void DrawPlayersAtY(int y, SpriteBatch spriteBatch)
    {
        foreach (var player in _world.Players
                     .Where(p => (int)p.Position.Y == y)
                     .Where(p => p.Position.X > _viewport.Left && p.Position.X < _viewport.Right)
                     .OrderBy(p => p.Position.Y))
        {
            DrawPlayer(player, spriteBatch);
        }
    }

    private void DrawFarm(Farm farm, SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_textures.GetTexture(farm.TextureName), _drawRect, Color.White );
        var connectionWidth = _screenAdapter.GetScreenLength(_textureSidePart);

        var topNeighbour = farm.Position.Y != 0 && _map.GetStructureAt(farm.Position.TopNeighbour()) is Farm;
        if (topNeighbour)
            DrawFarmYConnection(farm, connectionWidth, spriteBatch);
        
        var leftNeighbour = farm.Position.X != 0 && _map.GetStructureAt(farm.Position.LeftNeighbour()) is Farm;
        if (leftNeighbour)
            DrawFarmXConnection(farm, connectionWidth, spriteBatch);

        if (leftNeighbour && topNeighbour && _map.GetStructureAt(farm.Position.TopLeftNeighbour()) is Farm)
            DrawFarmCornerConnection(farm, connectionWidth, spriteBatch);
    }

    private void DrawFarmYConnection(Farm farm, int connectionHeight, SpriteBatch spriteBatch)
    {
        var rect = new Rectangle(
            _drawRect.X,
            _drawRect.Y - connectionHeight,
            _screenAdapter.TileSize,
            connectionHeight * 2);
        spriteBatch.Draw(_textures.GetTexture(farm.YConnectionTexture), rect, Color.White );
    }
    
    private void DrawFarmXConnection(Farm farm, int connectionWidth, SpriteBatch spriteBatch)
    {
        var rect = new Rectangle(
            _drawRect.X - connectionWidth,
            _drawRect.Y,
            connectionWidth * 2,
            _screenAdapter.TileSize);
        spriteBatch.Draw(_textures.GetTexture(farm.XConnectionTexture), rect, Color.White );
    }
    
    private void DrawFarmCornerConnection(Farm farm, int connectionWidth, SpriteBatch spriteBatch)
    {
        var rect = new Rectangle(
            _drawRect.X - connectionWidth,
            _drawRect.Y - connectionWidth,
            connectionWidth * 2,
            connectionWidth * 2);
        spriteBatch.Draw(_textures.GetTexture(farm.CornerTexture), rect, Color.White );
    }

    private void DrawWall(Wall wall, SpriteBatch spriteBatch)
    {
        _sourceRect.Width = _textureSize;
        _sourceRect.Height = _textureSize * (wall.Height + 1);
        if (wall.Left != null && wall.Right != null)
            _sourceRect.X = _textureSize / 2;
        else if (wall.Right == null)
            _sourceRect.X = _textureSize;
        else if (wall.Left == null)
            _sourceRect.X = 0;
        else
            _sourceRect.X = _textureSize * 2;
        
        spriteBatch.Draw(_textures.GetTexture(wall.TextureName), _drawRect, _sourceRect, Color.White );
    }

    private void DrawPlayer(Player player, SpriteBatch spriteBatch)
    {
        _drawRect.Location = _screenAdapter.GetScreenPosition(player.Position - new Vector2(0.5f, 2f)).ToPoint();
        _drawRect.Size = (player.Size * _screenAdapter.TileSize).ToPoint();

        var playerTexture = _playerTextures.GetTexture(player.TextureName);
        if (player.LooksLeft())
            spriteBatch.Draw(
                playerTexture.Side,
                _drawRect,
                playerTexture.Side.Bounds,
                Color.White,
                0,
                new Vector2(0.5f, 0),
                SpriteEffects.FlipHorizontally,
                1);
        else if (player.LooksRight())
            spriteBatch.Draw(playerTexture.Side, _drawRect, Color.White);
        else if (player.LooksForward())
            spriteBatch.Draw(playerTexture.Front, _drawRect, Color.White);
        else if (player.LooksBack())
            spriteBatch.Draw(playerTexture.Back, _drawRect, Color.White);

        DrawPlayerName(player, spriteBatch);
    }

    private void DrawPlayerName(Player player, SpriteBatch spriteBatch)
    {
        spriteBatch.DrawString(
            _fonts.MainFont,
            player.Name, 
            _drawRect.Location.ToVector2(),
            Color.White, 
            0f, 
            new Vector2(player.Name.Length * 5,16), 
            new Vector2(1,1),
            SpriteEffects.None,
            1);
    }

    private void DrawPointer(SpriteBatch spriteBatch)
    {
        _drawRect.Location = _screenAdapter.GetScreenPosition(_map.Pointer);
        _drawRect.Width = _screenAdapter.TileSize;
        _drawRect.Height = _screenAdapter.TileSize;
        if (_map.GetStructureAt(_map.Pointer) != null)
        {
            spriteBatch.Draw(_textures.BlankTexture(), _drawRect, new Color(Color.DarkRed, 0.2f));
        }
        else
        {
            spriteBatch.Draw(_textures.BlankTexture(), _drawRect, new Color(Color.Green, 0.2f));
        }
        
    }
}