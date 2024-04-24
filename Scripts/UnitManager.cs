using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using C3.MonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameDirectX;

public class UnitManager
{
    private List<Unit> _units;
    private List<Unit> _selectedUnits;

    private float _targetZoneRadiusMultiplier = 3.5f;
    private int currentId = 0;

    public UnitManager()
    {
        _units = new List<Unit>();
        _selectedUnits = new List<Unit>();
        InputManager.Instance.OnAddUnits += AddUnit;
        InputManager.Instance.OnSelecting += UnitsSelection;
        InputManager.Instance.OnRemoveUnits += RemoveSelectedUnits;
        InputManager.Instance.OnUnitsMove += ChangeUnitsTargetPosition;
    }

    private void RemoveSelectedUnits()
    {
        foreach (var unit in _selectedUnits)
        {
            _units.Remove(unit);
        }

        _selectedUnits.Clear();
    }

    private void UnitsSelection(Rectangle selectionRectangle)
    {
        _selectedUnits.Clear();
        foreach (var unit in _units)
        {
            if (unit.position.X > selectionRectangle.Left
                && unit.position.X < selectionRectangle.Right
                && unit.position.Y < selectionRectangle.Bottom
                && unit.position.Y > selectionRectangle.Top)
            {
                unit.isSelected = true;
                _selectedUnits.Add(unit);
            }
            else
            {
                unit.isSelected = false;
            }
        }
    }

    private void AddUnit(Vector2 mousePosition)
    {
        _units.Add(new Unit(mousePosition, currentId));
        currentId++;
    }

    private Vector2 RandomInCircle(Random random, float _radius)
    {
        var angle = random.NextDouble() * Math.PI * 2;
        var radius = random.NextDouble() * _radius;
        var x = radius * Math.Cos(angle);
        var y = radius * Math.Sin(angle);
        return new Vector2((float)x, (float)y);
    }

    public class Ray
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Dx { get; set; } // Direction x
        public float Dy { get; set; } // Direction y

        public Ray(float x, float y, float dx, float dy)
        {
            X = x;
            Y = y;
            Dx = dx;
            Dy = dy;
        }
    }

    public static Vector2 VoxelTraversal(Ray ray, int maxSteps, Unit unit)
    {
        unit.path.Clear();
        int x = (int)Math.Floor(ray.X);
        int y = (int)Math.Floor(ray.Y);

        // Directions to step in x and y (either +1 or -1)
        int stepX = ray.Dx > 0 ? 1 : -1;
        int stepY = ray.Dy > 0 ? 1 : -1;

        // Calculate step distances for x and y
        float tMaxX = (stepX > 0 ? (x + 1 - ray.X) : (ray.X - x)) / Math.Abs(ray.Dx);
        float tMaxY = (stepY > 0 ? (y + 1 - ray.Y) : (ray.Y - y)) / Math.Abs(ray.Dy);

        float tDeltaX = 1.0f / Math.Abs(ray.Dx);
        float tDeltaY = 1.0f / Math.Abs(ray.Dy);

        // Traverse the grid
        for (int i = 0; i < maxSteps; i++)
        {
            unit.path.Add(new Vector2(x, y) * GameWorld.TileSize);

            if (GameWorld.Instance.worldGrid[x, y].objectId != 0)
                return new Vector2(x, y) * GameWorld.TileSize;

            if (tMaxX < tMaxY)
            {
                tMaxX += tDeltaX;
                x += stepX;
            }
            else
            {
                tMaxY += tDeltaY;
                y += stepY;
            }
        }

        return new Vector2(x, y) * GameWorld.TileSize;
    }

    private void ChangeUnitsTargetPosition(Vector2 targetPosition)
    {
        foreach (var unit in _selectedUnits)
        {
            Vector2 direction = Vector2.Normalize(targetPosition - unit.position);
            float distance = Vector2.Distance(unit.position, targetPosition);
            var ray = new Ray(unit.position.X /GameWorld.TileSize, unit.position.Y/GameWorld.TileSize, direction.X, direction.Y);
            Vector2 intersectionPoint = VoxelTraversal(ray, (int)distance, unit);

            unit.targetPosition = intersectionPoint;
            unit.isSelected = false;
        }

        _selectedUnits.Clear();
    }

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

            for (int i = 1; i < unit.path.Count; i++)
            {
                Primitives2D.DrawRectangle(spriteBatch, unit.path[i], Vector2.One * 31, Color.GhostWhite);
            }
        }
    }
}