﻿using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameDirectX;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;

    private SpriteBatch _spriteBatch;

    private Effect backgroundShader;
    // private RenderTarget2D renderTarget;

    private GameWorld _gameWorld;
    private Camera _camera;


    private bool _drawRectangle;
    private Rectangle _selectionRectangle;

    public static int ScreenHeight = 1080;
    public static int ScreenWidth = 1920;

    private Matrix ProjectionMatrix;
    private Matrix InverseProjectionMatrix;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        ProjectionMatrix = Matrix.CreateOrthographicOffCenter(0, ScreenWidth, ScreenHeight, 0, -1, 1);
        InverseProjectionMatrix = Matrix.Invert(ProjectionMatrix);
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
        Resource.Instance.LoadContent(Content, GraphicsDevice);

        backgroundShader = Content.Load<Effect>("BackgroundShader");
        
        backgroundShader.Parameters["ScreenDimensions"]?.SetValue(new Vector2(ScreenWidth, ScreenHeight));

        // renderTarget = new RenderTarget2D(GraphicsDevice,
        //     GraphicsDevice.PresentationParameters.BackBufferWidth,
        //     GraphicsDevice.PresentationParameters.BackBufferHeight);
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

    private void DrawRectangle(Rectangle selectionRectangle)
    {
        _selectionRectangle = selectionRectangle;
        _drawRectangle = true;
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, null, null, null, backgroundShader);


        // Set the parameter in the shader


        backgroundShader.Parameters["InverseProjectionMatrix"]?.SetValue(InverseProjectionMatrix);
        backgroundShader.Parameters["ProjectionMatrix"]?.SetValue(ProjectionMatrix);
        
        backgroundShader.Parameters["ViewMatrix"]?.SetValue(_camera.ViewMatrix);
        backgroundShader.Parameters["InverseViewMatrix"]?.SetValue(_camera.InverseViewMatrix);

        //Multiple texture setup 
        // _spriteBatch.GraphicsDevice.Textures[0] = Resource.Instance.tilesTexture;
        backgroundShader.CurrentTechnique.Passes[0].Apply();

        _spriteBatch.Draw(Resource.Instance.pixel, new Rectangle(0, 0, ScreenWidth, ScreenHeight), Color.White);
        _spriteBatch.End();


        _spriteBatch.Begin(SpriteSortMode.Deferred,
            BlendState.AlphaBlend, null,
            null, null,
            null, _camera.ViewMatrix);


        if (_drawRectangle)
        {
            _spriteBatch.Draw(Resource.Instance.pixel, _selectionRectangle, Color.White);
            _drawRectangle = false;
        }

        _gameWorld.Draw(_spriteBatch);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}