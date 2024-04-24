using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameDirectX;

public class Unit
{
    private static readonly Vector2 Offset = new Vector2(16, 16);
    private float speed = 1.0f;
    public float hopping = 0.0f;
    private float hoppingFrequency = 10.0f;
    private float hoppingHeight = 5.0f;


    public int iD;
    public Vector2 position;
    public Vector2 targetPosition;
    public bool isSelected = false;

    public List<Vector2> path;

    public Unit(Vector2 position,int iD)
    {
        this.position = position;
        this.iD = iD;
        targetPosition = position;
        path = new List<Vector2>();
    }

    private void Move(GameTime gametime)
    {
        position.Y += hopping;

        Vector2 direction = targetPosition - position;
        if (direction.Length() >= 1.0f)
        {
            direction.Normalize();
            position += direction * speed;
        }

        var t = (float)(gametime.TotalGameTime.TotalSeconds + new Random(iD).NextDouble());
        hopping = (float)Math.Abs(Math.Sin(t * hoppingFrequency) * hoppingHeight);

        position.Y -= hopping;

    }

    public void Update(GameTime gametime)
    {
        Move(gametime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Resource.Instance.pixel, targetPosition - new Vector2(8,0), new Rectangle(0, 0, 16, 16),isSelected ? Color.Blue : Color.White);
        
        spriteBatch.Draw(Resource.Instance.unitTexture, position - Offset,
            new Rectangle(0, 0, 32, 32), isSelected ? Color.Blue : Color.White);
    }
}