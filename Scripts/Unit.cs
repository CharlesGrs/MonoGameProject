using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameDirectX;

public class Unit
{
    public Point position;
    private static readonly Vector2 Offset = new Vector2(16, 16);
    public bool isSelected = false;

    public Unit(Point position)
    {
        this.position = position;
    }

    public void Update(GameTime gametime)
    {
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Resource.Instance.unitTexture, position.ToVector2() - Offset,
            new Rectangle(0, 0, 32, 32), isSelected ? Color.Blue : Color.White);
    }
}