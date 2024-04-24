using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameDirectX;

public class Resource
{
    private static Resource instance;
    public static Resource Instance => instance ??= new Resource();

    public Texture2D unitTexture;
    public Texture2D tilesTexture;
    public Texture2D pixel;

    
    public void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice)
    {
        unitTexture = contentManager.Load<Texture2D>("rogues");
        tilesTexture = contentManager.Load<Texture2D>("tiles");
        
        pixel = new Texture2D(graphicsDevice, 1, 1);
        pixel.SetData(new Color[] { new Color(.7f,.7f,.7f) });
    }
}