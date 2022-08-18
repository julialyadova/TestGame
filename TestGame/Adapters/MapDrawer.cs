using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TestGame.Core.Entities.Creatures;
using TestGame.Core.Entities.Structures;
using TestGame.Entities;
using TestGame.Tools;

namespace TestGame.Adapters;

public class MapDrawer
{
    private const int TextureSize = 128;
    private WorldMap _map;
    private Player _player;
    private MapToScreenAdapter _screenAdapter;
    private MapTexturesRepository _textures;

    private Rectangle _drawRect = Rectangle.Empty;
    private Rectangle _sourceRect = Rectangle.Empty;

    public MapDrawer(IServiceProvider services)
    {
        _map = services.GetRequiredService<WorldMap>();
        _screenAdapter = services.GetRequiredService<MapToScreenAdapter>();
        _textures = services.GetRequiredService<MapTexturesRepository>();
        _player = services.GetRequiredService<Player>();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        DrawSurfaces(spriteBatch);
        DrawStructures(spriteBatch);
        //DrawPointer(spriteBatch);
    }

    void DrawSurfaces(SpriteBatch spriteBatch)
    {
        _drawRect.Width = _screenAdapter.TileSize;
        _drawRect.Height = _screenAdapter.TileSize;
        
        for (int mapX = 0; mapX < _map.Size.X; mapX++)
        for (int mapY = 0; mapY < _map.Size.Y; mapY++)
        {
            var surface = _map.SurfacesMap[mapX, mapY];
            _drawRect.Location = _screenAdapter.GetScreenPosition(new Point(mapX, mapY));
            spriteBatch.Draw(_textures.GetTexture(surface.TextureName), _drawRect, Color.White );
        }
    }

    private void DrawStructures(SpriteBatch spriteBatch)
    {
        for (int level = 0; level < _map.Size.Y; level++)
        {
            if ((int)_player.Position.Y == level)
                DrawPlayer(spriteBatch);
                
            if (_map.Structures[level] == null)
                continue;

            foreach (var structure in _map.Structures[level])
            {
            
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
        _sourceRect.Width = TextureSize;
        _sourceRect.Height = TextureSize * (wall.Height + 1);
        if (wall.Left != null && wall.Right != null)
            _sourceRect.X = TextureSize / 2;
        else if (wall.Right == null)
            _sourceRect.X = TextureSize;
        else if (wall.Left == null)
            _sourceRect.X = 0;
        else
            _sourceRect.X = TextureSize * 2;
        
        spriteBatch.Draw(_textures.GetTexture(wall.TextureName), _drawRect, _sourceRect, Color.White );
    }

    private void DrawPlayer(SpriteBatch spriteBatch)
    {
        var playerSizeOffset = new Vector2(_player.Size.X / 2, _player.Size.Y);
        _drawRect.Location = _screenAdapter.GetScreenPosition(_player.Position - playerSizeOffset).ToPoint();
        _drawRect.Size = (_player.Size * _screenAdapter.TileSize).ToPoint();
        spriteBatch.Draw(_textures.BlankTexture(), _drawRect, Color.BurlyWood);
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