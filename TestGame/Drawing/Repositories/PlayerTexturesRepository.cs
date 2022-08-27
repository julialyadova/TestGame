using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TestGame.Services;

namespace TestGame.Drawing.Repositories;

public class PlayerTexturesRepository : ContentRepository
{
    private readonly string texturePath = "Textures/Players/";
    private readonly string frontSuffix = "_f";
    private readonly string backSuffix = "_b";
    private readonly string sideSuffix = "_s";
    
    private List<string> textureNames = new ()
    {
        "player"
    };
    
    private Dictionary<string, PlayerTexture> textures = new ();

    public PlayerTexturesRepository()
    {
        foreach (var name in textureNames)
        {
            textures[name] = new PlayerTexture(
                ContentManager.Load<Texture2D>(texturePath + name + frontSuffix),
                ContentManager.Load<Texture2D>(texturePath + name + backSuffix)
,                ContentManager.Load<Texture2D>(texturePath + name + sideSuffix)) ;
        }
    }

    public PlayerTexture GetTexture(string name)
    {
        return name != null && textures.ContainsKey(name) ? textures[name] : textures["player"];
    }
}