using Microsoft.Xna.Framework;

namespace MonoGameDirectX;

public class Camera
{
    public const float Speed = 10.0f;
    public Matrix Transform { get; private set; }
    private Vector2 Position { get; set; }
    private float _zoom;

    public Camera()
    {
        Position = new Vector2(Game1.ScreenWidth / 2, Game1.ScreenHeight / 2);
        _zoom = 1.0f;

        InputManager.Instance.OnMove += Move;
    }

    private void Move(Vector2 offset)
    {
        Position += offset * Speed;
    }

    private void UpdateTransform()
    {
        Transform = Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
                    Matrix.CreateScale(new Vector3(_zoom, _zoom, 1)) *
                    Matrix.CreateTranslation(new Vector3(Game1.ScreenWidth / 2, Game1.ScreenHeight / 2, 0));
    }

    public void Update(GameTime gametime)
    {
        UpdateTransform();
    }
}