using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Linq;

namespace GGJRuntime
{
    public class InputImageParameters
    {
        private Vector2Int? bottomRightTileCoords = null;
        private Vector2Int? topLeftTileCoords = null;
        private BoundsInt inputTileMapBounds;
        private Tilemap tilemap = null;
        private TileBase[] inputTilemapTiles = null;
        private Queue<TileContainer> stackOfTiles = new Queue<TileContainer>();
        private int width = 0;
        private int height = 0;

        public Queue<TileContainer> StackOfTiles
        {
            get => stackOfTiles;
            set => stackOfTiles = value;
        }

        public int Width { get => width; }

        public int Height {  get => height;  }


        public InputImageParameters(Tilemap tilemap)
        {
            this.tilemap = tilemap;
            this.inputTileMapBounds = tilemap.cellBounds;
            this.inputTilemapTiles = tilemap.GetTilesBlock(this.inputTileMapBounds);

            ExtractNonEmptyTiles();
            VerifyInputTiles();
        }


        private void VerifyInputTiles()
        {
            if(topLeftTileCoords == null || bottomRightTileCoords == null)
            {
                throw new System.Exception("WFC: Input tilemap is empty!");
            }

            int minX = bottomRightTileCoords.Value.x;
            int maxX = topLeftTileCoords.Value.x;
            int minY = bottomRightTileCoords.Value.y;
            int maxY = topLeftTileCoords.Value.y;

            width = Mathf.Abs(maxX - minX) + 1;
            height = Mathf.Abs(maxY - minY) + 1;

            int tileCount = width * height;

            if(stackOfTiles.Count != tileCount)
            {
                throw new System.Exception("WFC: Tilemap has gaps!");
            }

            if(stackOfTiles.Any(tile => tile.X > maxX || tile.X < minX || tile.Y > maxY || tile.Y < minY))
            {
                throw new System.Exception("WFC: Tilemap should be a filled rect!");
            }
        }


        private void ExtractNonEmptyTiles()
        {
            for(int row=0; row < inputTileMapBounds.size.y; row++)
            {
                for(int col=0; col < inputTileMapBounds.size.x; col++)
                {
                    int index = col + (row * inputTileMapBounds.size.x);
                    TileBase tile = inputTilemapTiles[index];

                    if(bottomRightTileCoords == null && tile != null)
                    {
                        bottomRightTileCoords = new Vector2Int(col, row);
                    }

                    if(tile != null)
                    {
                        stackOfTiles.Enqueue(new TileContainer(tile, col, row));
                        topLeftTileCoords = new Vector2Int(col, row);
                    }
                }
            }
        }
    }
}