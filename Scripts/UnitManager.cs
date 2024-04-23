using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameDirectX;

public class UnitManager
{
    private List<Unit> _units;
    
    
    public void Update(GameTime gametime)
    {
        foreach (var unit in _units)
        {
            unit.Update(gametime);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var unit in _units)
        {
            unit.Draw(spriteBatch);
        }
    }

}