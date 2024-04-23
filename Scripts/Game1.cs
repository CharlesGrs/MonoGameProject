using System;
using System.Net.Mime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameDirectX;

public class Game1 : Game
{
    // private Texture2D pixel;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    // private Effect grayscaleEffect;
    // private RenderTarget2D renderTarget;

    private GameWorld _gameWorld;
    private Camera _camera;
    private InputManager _inputManager;


    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _inputManager = new InputManager();
        _camera = new Camera();
        _gameWorld = new GameWorld();

        _inputManager.Onclick += _gameWorld.Test;
        
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // grayscaleEffect = Content.Load<Effect>("GrayscaleEffect");
        // renderTarget = new RenderTarget2D(GraphicsDevice,
        //     GraphicsDevice.PresentationParameters.BackBufferWidth,
        //     GraphicsDevice.PresentationParameters.BackBufferHeight);


        // pixel = new Texture2D(GraphicsDevice, 1, 1);
        // pixel.SetData(new Color[] { Color.White });
    }

    private Point startPoint;
    private Point endPoint;
    private Rectangle selectionRectangle;
    private bool isSelecting = false;

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        
        _gameWorld.Update(gameTime);
        _camera.Update(gameTime);
        _inputManager.Update(gameTime);

        // var kState = Keyboard.GetState();
        // var mState = Mouse.GetState();
        //
        //
        // if (mState.LeftButton == ButtonState.Pressed)
        // {
        //     if (!isSelecting)
        //     {
        //         startPoint = mState.Position;
        //         isSelecting = true;
        //     }
        //
        //     endPoint = mState.Position;
        //
        //     int x = Math.Min(startPoint.X, endPoint.X);
        //     int y = Math.Min(startPoint.Y, endPoint.Y);
        //     int width = Math.Abs(startPoint.X - endPoint.X);
        //     int height = Math.Abs(startPoint.Y - endPoint.Y);
        //     selectionRectangle = new Rectangle(x, y, width, height);
        // }
        // else
        // {
        //     isSelecting = false;
        // }


        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // _gameWorld.Draw(_spriteBatch);

        base.Draw(gameTime);
    }
}