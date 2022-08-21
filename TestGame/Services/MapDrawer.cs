using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TestGame.Core.Entities.Creatures;
using TestGame.Core.Entities.Structures;
using TestGame.Core.Map;
using TestGame.UI;

namespace TestGame.Adapters;

public class MapDrawer
{
    private readonly int _textureSize = 128;
    private readonly int _maxStructSize = 10;
    private WorldMap _map;
    private MapToScreenAdapter _screenAdapter;
    private MapTexturesRepository _textures;
    private FontsRepository _fonts;

    private Rectangle _drawRect = Rectangle.Empty;
    private Rectangle _sourceRect = Rectangle.Empty;
    private Rectangle _viewport = Rectangle.Empty;

    public MapDrawer(IServiceProvider services)
    {
        _map = services.GetRequiredService<WorldMap>();
        _screenAdapter = services.GetRequiredService<MapToScreenAdapter>();
        _textures = services.GetRequiredService<MapTexturesRepository>();
        _fonts = services.GetRequiredService<FontsRepository>();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _viewport = _screenAdapter.MapViewport;
        if (_map.Loaded)
        {
            DrawSurfaces(spriteBatch);
            DrawStructures(spriteBatch);
        }
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
        for (int level = _viewport.Y; level < _viewport.Bottom + _maxStructSize; level++)
        {
            foreach (var player in _map.Players)
            {
                if ((int)player.Position.Y == level)
                    DrawPlayer(player, spriteBatch);
            }

            if (_map.Structures[level] == null)
                continue;

            foreach (var structure in _map.Structures[level])
            {
                
                if (structure.Position.X < _viewport.X - _maxStructSize || structure.Position.X >= _viewport.Right)
                    continue;

                _drawRect.Location = _screenAdapter.GetScreenPosition(new Point(structure.Position.X, structure.Position.Y - structure.Height));
                _drawRect.Height = _screenAdapter.GetScreenLength(structure.Size.Y + structure.Height);
                _drawRect.Width = _screenAdapter.GetScreenLength(structure.Size.X);
    
                if (structure is Wall wall)
                    DrawWall(wall, spriteBatch);
                else
                    spriteBatch.Draw(_textures.GetTexture(structure.TextureName), _drawRect, Color.White );
            }
        }
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
        
        var playerSizeOffset = new Vector2(player.Size.X / 2, player.Size.Y);
        _drawRect.Location = _screenAdapter.GetScreenPosition(player.Position - playerSizeOffset).ToPoint();
        _drawRect.Size = (player.Size * _screenAdapter.TileSize).ToPoint();
        spriteBatch.Draw(_textures.GetTexture(player.TextureName), _drawRect, Color.BurlyWood);
        _drawRect.X += _drawRect.Width / 2;
        spriteBatch.DrawString(
            _fonts.MainFont,
            player.Name, 
            _drawRect.Location.ToVector2(),
            Color.White, 
            0f, 
            new Vector2(player.Name.Length * 4,16), 
            new Vector2(1,1),
            SpriteEffects.None,
            1);
    }

    private void DrawPointer(SpriteBatch spriteBatch)
    {
        _drawRect.Location = _screenAdapter.GetScreenPosition(_map.Pointer);
        _drawRect.Width = _screenAdapter.TileSize;
        _drawRect.Height = _screenAdapter.TileSize;
        if (_map.StructuresMap[_map.Pointer.X, _map.Pointer.Y] != null)
        {
            spriteBatch.Draw(_textures.BlankTexture(), _drawRect, new Color(Color.DarkRed, 0.2f));
        }
        else
        {
            spriteBatch.Draw(_textures.BlankTexture(), _drawRect, new Color(Color.Green, 0.2f));
        }
        
    }
}