using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D11;
using static System.MathF;

namespace MonoGameDirectX;

public struct Tile
{
    public byte tileIndex;
    public byte objectId;
    public bool isOccupied;

    public Tile(byte tileIndex, byte objectId)
    {
        this.objectId = objectId;
        this.tileIndex = tileIndex;
        isOccupied = false;
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
        if (t.isOccupied)
            return Color.Red;
        if (t.tileIndex == 0)
        {
            return Color.Green;
        }

        return Color.Brown;
    }

    public void SetIsOccupied(Vector2 worldPos, bool state)
    {
        var x = (int)Floor(worldPos.X / TileSize);
        var y = (int)Floor(worldPos.Y / TileSize);

        if (x > WorldWidth - 1 || x < 0)
            return;
        if (y > WorldHeight - 1 || y < 0)
            return;

        worldGrid[x, y].isOccupied = state;
    }

    public readonly Func<Vector2, bool> isTileOccupied = v => instance.GetIsOccupied(v);

    public Vector2 FindNearestTileWorldPos(Vector2 startPosition, Func<Vector2, bool> condition)
    {
        bool found = false;
        int radius = 1;

        List<Vector2> possiblePositions = new List<Vector2>();

        while (!found)
        {
            for (int x = -radius; x < radius + 1; x++)
            {
                for (int y = -radius; y < radius + 1; y++)
                {
                    if (!condition(startPosition + new Vector2(x * TileSize, y * TileSize)))
                    {
                        found = true;
                        possiblePositions.Add(new Vector2(x * TileSize, y * TileSize));
                    }
                }
            }

            radius++;
        }

        Random random = new Random();
        var tilePosition = possiblePositions[random.Next(0, possiblePositions.Count)] + startPosition;
        return tilePosition;
    }

    public bool GetIsOccupied(Vector2 worldPos)
    {
        var x = (int)Floor(worldPos.X / TileSize);
        var y = (int)Floor(worldPos.Y / TileSize);

        if (x > WorldWidth - 1 || x < 0)
            return true;
        if (y > WorldHeight - 1 || y < 0)
            return true;

        return worldGrid[x, y].isOccupied;
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