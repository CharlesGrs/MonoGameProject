using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGameDirectX;

public class InputManager
{
    #region PRIVATE FIELDS

    private bool isSelecting;
    private Point startPoint;
    private Point endPoint;
    private Rectangle selectionRectangle;

    #endregion

    #region PUBLIC FIELDS

    public Camera mainCamera;
    public event Action<Vector2> OnMove;
    public event Action<Point> OnAddUnits;
    public event Action OnRemoveUnits;
    public event Action<Rectangle> OnSelecting;

    #endregion

    #region SINGLETON

    private static InputManager instance;
    public static InputManager Instance => instance ??= new InputManager();

    #endregion

    public void Update(GameTime gametime)
    {
        var mouseState = Mouse.GetState();
        var keyboardState = Keyboard.GetState();

        HandleCameraInputs(keyboardState, mouseState);

        HandleUnitsInputs(keyboardState, mouseState);

        HandleSelection(mouseState);
    }

    private void HandleCameraInputs(KeyboardState keyboardState, MouseState mouseState)
    {
        var moveVector = Vector2.Zero;
        if (keyboardState.IsKeyDown(Keys.Z))
            moveVector.Y--;

        if (keyboardState.IsKeyDown(Keys.Q))
            moveVector.X--;

        if (keyboardState.IsKeyDown(Keys.D))
            moveVector.X++;

        if (keyboardState.IsKeyDown(Keys.S))
            moveVector.Y++;

        OnMove?.Invoke(moveVector);
    }

    private void HandleUnitsInputs(KeyboardState keyboardState, MouseState mouseState)
    {
        if (keyboardState.IsKeyDown(Keys.A))
        {
            OnAddUnits?.Invoke(GetWorldMousePosition(mouseState).ToPoint());
        }

        if (keyboardState.IsKeyDown(Keys.R))
        {
            OnRemoveUnits?.Invoke();
        }
    }

    private void HandleSelection(MouseState mouseState)
    {
        if (mainCamera == null) return;
        var mousePosition = GetWorldMousePosition(mouseState);
        if (mouseState.LeftButton == ButtonState.Pressed)
        {
            if (!isSelecting)
            {
                startPoint = mousePosition.ToPoint();
                isSelecting = true;
            }

            endPoint = mousePosition.ToPoint();

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

    public Vector2 GetWorldMousePosition(MouseState mouseState)
    {
        var invertedTransform = Matrix.Invert(mainCamera.Transform);
        var worldPosition = Vector2.Transform(mouseState.Position.ToVector2(), invertedTransform);
        return worldPosition;
    }
}