using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameDirectX;

public class GameWorld
{
    
    private UnitManager _unitManager;

    public GameWorld()
    {
        _unitManager = new UnitManager();
    }

    public void Update(GameTime gametime)
    {
        _unitManager.Update(gametime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _unitManager.Draw(spriteBatch);
    }

    public void Test()
    {
        Debug.WriteLine("it worked");
    }

}