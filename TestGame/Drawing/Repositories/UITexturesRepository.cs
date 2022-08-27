using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TestGame.Services;

namespace TestGame.Drawing.Repositories;

public class UITexturesRepository : ContentRepository
{
    private List<string> textureNames = new ()
    {
        "Textures/UI/btn_host",
        "Textures/UI/btn_join",
        "Textures/UI/btn_exit"
    };
    private Dictionary<string, Texture2D> textures = new ();
    private Texture2D _undefinedTexture;

    public UITexturesRepository()
    {
        _undefinedTexture = new Texture2D(GraphicsDevice, 1, 1);
        _undefinedTexture.SetData(new []{Color.Wheat});
        
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