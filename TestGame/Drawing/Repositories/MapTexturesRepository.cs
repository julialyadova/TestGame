using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TestGame.Services;

namespace TestGame.Drawing.Repositories;

public class MapTexturesRepository : ContentRepository
{
    private List<string> textureNames = new ()
    {
        "Textures/World/grass",
        "Textures/Structures/wall",
        "Textures/Structures/brick_wall",
        "Textures/Structures/Trees/tree",
        "Textures/Structures/Trees/high_tree",
        "Textures/Structures/Farm/farm",
        "Textures/Structures/Farm/farm_connect_x",
        "Textures/Structures/Farm/farm_connect_y",
        "Textures/Structures/Farm/farm_corner"
    };
    private Dictionary<string, Texture2D> textures = new ();
    private Texture2D _undefinedTexture;

    public MapTexturesRepository()
    {
        _undefinedTexture = new Texture2D(GraphicsDevice, 1, 1);
        _undefinedTexture.SetData(new []{Color.Fuchsia});
        
        foreach (var name in textureNames)
        {
            textures[name] = ContentManager.Load<Texture2D>(name);
        }
    }

    public Texture2D GetTexture(string name)
    {
        return name != null && textures.ContainsKey(name) ? textures[name] : _undefinedTexture;
    }

    public Texture2D BlankTexture()
    {
        return _undefinedTexture;
    }
}