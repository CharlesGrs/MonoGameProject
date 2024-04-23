using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameDirectX;

public class Game1 : Game
{
    private Texture2D pixel;
    private GraphicsDeviceManager _graphics;

    private SpriteBatch _spriteBatch;
    // private Effect grayscaleEffect;
    // private RenderTarget2D renderTarget;

    private GameWorld _gameWorld;
    private Camera _camera;

    public static int ScreenHeight = 1080;
    public static int ScreenWidth = 1920;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        Debug.WriteLine("Init");
        _camera = new Camera();
        _gameWorld = new GameWorld();

        _graphics.IsFullScreen = false;
        _graphics.PreferredBackBufferWidth = ScreenWidth;
        _graphics.PreferredBackBufferHeight = ScreenHeight;
        _graphics.ApplyChanges();

        InputManager.Instance.OnSelecting += DrawRectangle;
        InputManager.Instance.mainCamera = _camera;
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        Resource.Instance.LoadContent(Content);
        // grayscaleEffect = Content.Load<Effect>("GrayscaleEffect");
        // renderTarget = new RenderTarget2D(GraphicsDevice,
        //     GraphicsDevice.PresentationParameters.BackBufferWidth,
        //     GraphicsDevice.PresentationParameters.BackBufferHeight);


        pixel = new Texture2D(GraphicsDevice, 1, 1);
        pixel.SetData(new Color[] { Color.White });
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        _gameWorld.Update(gameTime);
        _camera.Update(gameTime);
        InputManager.Instance.Update(gameTime);


        base.Update(gameTime);
    }

    private bool _drawRectangle;
    private Rectangle _selectionRectangle;

    private void DrawRectangle(Rectangle selectionRectangle)
    {
        _selectionRectangle = selectionRectangle;
        _drawRectangle = true;
        Debug.WriteLine("Test");
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin(SpriteSortMode.Deferred,
            BlendState.AlphaBlend, null,
            null, null,
            null, _camera.Transform);

        if (_drawRectangle)
        {
            _spriteBatch.Draw(pixel, _selectionRectangle, Color.White);
            _drawRectangle = false;
        }

        _gameWorld.Draw(_spriteBatch);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}