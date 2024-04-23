using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameDirectX;

public class Resource
{
    private static Resource instance;
    public static Resource Instance => instance ??= new Resource();

    public Texture2D unitTexture;
    
    public void LoadContent(ContentManager contentManager)
    {
        unitTexture = contentManager.Load<Texture2D>("rogues");
    }
}