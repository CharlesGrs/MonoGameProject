using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        return new Vector2((float)x,(float)y);
    }

    private void ChangeUnitsTargetPosition(Vector2 targetPositionCenter)
    {
        Random rand = new Random();

        foreach (var units in _selectedUnits)
        {
            units.targetPosition = targetPositionCenter + RandomInCircle(rand, _selectedUnits.Count * _targetZoneRadiusMultiplier);
            units.isSelected = false;
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
        _units.Sort((u1, u2) => (u1.position.Y - u1.hopping).CompareTo(u2.position.Y - u2.hopping));
         
        foreach (var unit in _units)
        {
            spriteBatch.Draw(Resource.Instance.pixel, unit.targetPosition - new Vector2(8,0), new Rectangle(0, 0, 16, 16),unit.isSelected ? Color.Blue : Color.White);
        }
        
        foreach (var unit in _units)
        {
            spriteBatch.Draw(Resource.Instance.unitTexture, unit.position - new Vector2(16,16),
                new Rectangle(0, 0, 32, 32), unit.isSelected ? Color.Blue : Color.White);
        }
       
        
       
    }
}