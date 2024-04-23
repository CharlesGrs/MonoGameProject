using System;
using System.Net.Mime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameDirectX;

public class Game1 : Game
{
    private Texture2D pixel;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Effect grayscaleEffect;
    private RenderTarget2D renderTarget;


    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        grayscaleEffect = Content.Load<Effect>("GrayscaleEffect");
        renderTarget = new RenderTarget2D(GraphicsDevice,
            GraphicsDevice.PresentationParameters.BackBufferWidth,
            GraphicsDevice.PresentationParameters.BackBufferHeight);


        pixel = new Texture2D(GraphicsDevice, 1, 1);
        pixel.SetData(new Color[] { Color.White });
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

        var kState = Keyboard.GetState();
        var mState = Mouse.GetState();


        if (mState.LeftButton == ButtonState.Pressed)
        {
            if (!isSelecting)
            {
                startPoint = mState.Position;
                isSelecting = true;
            }

            endPoint = mState.Position;

            int x = Math.Min(startPoint.X, endPoint.X);
            int y = Math.Min(startPoint.Y, endPoint.Y);
            int width = Math.Abs(startPoint.X - endPoint.X);
            int height = Math.Abs(startPoint.Y - endPoint.Y);
            selectionRectangle = new Rectangle(x, y, width, height);
        }
        else
        {
            isSelecting = false;
        }


        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.SetRenderTarget(renderTarget);
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
        if (isSelecting)
        {
            _spriteBatch.Draw(pixel, selectionRectangle, Color.Wheat);
        }

        _spriteBatch.End();
        GraphicsDevice.SetRenderTarget(null);

        _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque,
            null, null, null, grayscaleEffect);
        _spriteBatch.Draw(renderTarget, new Rectangle(0, 0, renderTarget.Width, renderTarget.Height), Color.White);
        _spriteBatch.End();


        base.Draw(gameTime);
    }
}