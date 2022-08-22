using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TestGame.Drawing.Repositories;

public interface IContentRepository
{
    void LoadContent(GraphicsDevice device, ContentManager contentManager);
}