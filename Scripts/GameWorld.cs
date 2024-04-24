using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameDirectX;

public struct Tile
{
    public byte tileIndex;
    public byte objectId;

    public Tile(byte tileIndex, byte objectId)
    {
        this.objectId = objectId;
        this.tileIndex = tileIndex;
    }
}

public class GameWorld
{
    private static GameWorld instance;
    public static GameWorld Instance => instance ??= new GameWorld();

    private UnitManager _unitManager;
    public static int WorldWidth = 60;
    public static int WorldHeight = 34;
    public static int TileSize = 32;
    public Tile[,] worldGrid;

    private GameWorld()
    {
        _unitManager = new UnitManager();
        GenerateTileGrid();
    }

    private void GenerateTileGrid()
    {
        FastNoise noise = new FastNoise();
        noise.SetNoiseType(FastNoise.NoiseType.Simplex);

        worldGrid = new Tile[WorldWidth, WorldHeight];

        var random = new Random();
        for (int x = 0; x < WorldWidth; x++)
        {
            for (int y = 0; y < WorldHeight; y++)
            {
                float tileTypeNoise = noise.GetNoise(x * 5, y * 5);
                float forestNoise = noise.GetNoise(x * 3, y * 3);

                float prob = random.NextSingle();
                byte objectId = 0;
                if (forestNoise < .2f)
                {
                    objectId = 1;
                }

                var tile = new Tile((byte)Math.Round(tileTypeNoise), objectId);


                worldGrid[x, y] = tile;
            }
        }
    }

    public void Update(GameTime gametime)
    {
        _unitManager.Update(gametime);
    }

    Color TileColor(Tile t)
    {
        if (t.tileIndex == 0)
            return Color.Green;

        return Color.Brown;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        DrawWorld(spriteBatch);

        _unitManager.Draw(spriteBatch);
    }

    private void DrawWorld(SpriteBatch spriteBatch)
    {
        for (int i = 0; i < WorldWidth; i++)
        {
            for (int j = 0; j < WorldHeight; j++)
            {
                var tile = worldGrid[i, j];
                spriteBatch.Draw(Resource.Instance.pixel,
                    new Rectangle(i * TileSize, j * TileSize, TileSize, TileSize),
                    TileColor(tile));

                if (tile.objectId != 0)
                {
                    spriteBatch.Draw(Resource.Instance.pixel,
                        new Rectangle(i * TileSize + TileSize / 4, j * TileSize + TileSize / 4, TileSize / 2,
                            TileSize / 2),
                        Color.Chartreuse);
                }
            }
        }
    }
}