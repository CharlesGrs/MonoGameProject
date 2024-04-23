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

    public UnitManager()
    {
        _units = new List<Unit>();
        _selectedUnits = new List<Unit>();
        InputManager.Instance.OnAddUnits += AddUnit;
        InputManager.Instance.OnSelecting += TestSelectionUnits;
        InputManager.Instance.OnRemoveUnits += RemoveSelectedUnits;
    }

    private void RemoveSelectedUnits()
    {
        foreach (var unit in _selectedUnits)
        {
            _units.Remove(unit);
        }
        _selectedUnits.Clear();
    }

    private void TestSelectionUnits(Rectangle selectionRectangle)
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

    private void AddUnit(Point mousePosition)
    {
        _units.Add(new Unit(mousePosition));
        Debug.Write(_units.Count.ToString());
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
        }
    }
}