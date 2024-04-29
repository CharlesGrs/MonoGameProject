using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using C3.MonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameDirectX;

public class Unit
{
    private static readonly Vector2 Offset = new(16, 16);
    private float speed = 1.0f;
    public float hopping;
    private float hoppingFrequency = 10.0f;
    private float hoppingHeight = 5.0f;
    private bool isMoving;


    public int iD;
    private GameWorld gameWorld;
    public Vector2 position;
    private Vector2 targetPosition;

    public Vector2 TargetPosition
    {
        get => targetPosition;
        set
        {
            targetPosition = value;
            isMoving = true;
            GameWorld.Instance.SetIsOccupied(position, false);
        }
    }

    public bool isSelected = false;

    public List<Vector2> path;

    public Unit(Vector2 position, int iD)
    {
        this.position = position;
        this.iD = iD;
        TargetPosition = position;
        path = new List<Vector2>();
        gameWorld = GameWorld.Instance;
    }

    private void Move(GameTime gametime)
    {
        // position.Y += hopping;

        Vector2 direction = targetPosition - position;
        float distanceToTarget = direction.Length();
        if (distanceToTarget >= 0.5f)
        {
            position += Vector2.Normalize(direction) * speed;
            isMoving = true;
        }
        else
        {
            if (isMoving)
            {
                bool isOccupied = gameWorld.GetIsOccupied(position);
                if (isOccupied)
                {
                    var tilePosition = gameWorld.FindNearestTileWorldPos(position, gameWorld.isTileOccupied);
                    targetPosition = tilePosition;
                }
                else
                {
                    gameWorld.SetIsOccupied(position, true);
                    isMoving = false;
                }
            }
        }

        var t = gametime.TotalGameTime.TotalSeconds;
        hopping = (float)Math.Abs(Math.Sin(t * hoppingFrequency + iD) * hoppingHeight);

        // position.Y -= hopping;
    }

    public void Update(GameTime gametime)
    {
        Move(gametime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Primitives2D.DrawCircle(spriteBatch, targetPosition, 8f, 8, isSelected ? Color.Cyan : Color.White);
        spriteBatch.Draw(Resource.Instance.unitTexture, position - Offset,
            new Rectangle(0, 0, 32, 32), isSelected ? Color.Cyan : Color.White);
    }
}