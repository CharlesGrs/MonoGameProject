using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGameDirectX;

public class InputManager
{

    private bool isSelecting;
    private Point startPoint;
    private Point endPoint;
    private Rectangle selectionRectangle;
    
    private static InputManager instance;
    public static InputManager Instance => instance ??= new InputManager();
    public event Action<Point> OnButtonAPressed;
    public event Action OnButtonRPressed;
    public event Action<Rectangle> OnSelecting; 
    
    public void Update(GameTime gametime)
    {
        var mouseState = Mouse.GetState();
        var keyboardState = Keyboard.GetState();
        if (keyboardState.IsKeyDown(Keys.A))
        {
            OnButtonAPressed?.Invoke(mouseState.Position);
        }
        
        if (keyboardState.IsKeyDown(Keys.R))
        {
            OnButtonRPressed?.Invoke();
        }
        if (mouseState.LeftButton == ButtonState.Pressed)
        {
            if (!isSelecting)
            {
                startPoint = mouseState.Position;
                isSelecting = true;
            }
        
            endPoint = mouseState.Position;
        
            ComputeSelectionRectangle();
            
            OnSelecting?.Invoke(selectionRectangle);
        }
        else
        {
            isSelecting = false;
        }
    }

    private void ComputeSelectionRectangle()
    {
        int x = Math.Min(startPoint.X, endPoint.X);
        int y = Math.Min(startPoint.Y, endPoint.Y);
        int width = Math.Abs(startPoint.X - endPoint.X);
        int height = Math.Abs(startPoint.Y - endPoint.Y);
        selectionRectangle = new Rectangle(x, y, width, height);
    }
}