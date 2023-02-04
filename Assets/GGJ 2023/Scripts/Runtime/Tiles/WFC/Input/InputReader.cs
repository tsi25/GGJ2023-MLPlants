using UnityEngine;
using UnityEngine.Tilemaps;

namespace GGJRuntime
{
    public class InputReader : IInputReader<TileBase>
    {
        private Tilemap inputTilemap = null;


        public InputReader(Tilemap input)
        {
            inputTilemap = input;
        }

        public IValue<TileBase>[][] ReadInputToGrid()
        {
            TileBase[][] grid = ReadInputTileMap();

            if(grid == null) return null;

            TileBaseValue[][] gridOfValues = ArrayExtensions.CreateJaggedArray<TileBaseValue[][]>(grid.Length, grid[0].Length);
            
            for(int i=0; i < grid.Length; i++)
            {
                for(int j=0; j < grid[0].Length; j++)
                {
                    gridOfValues[i][j] = new TileBaseValue(grid[i][j]);
                }
            }

            return gridOfValues;
        }


        private TileBase[][] ReadInputTileMap()
        {
            return CreateTileBasedGrid(new InputImageParameters(inputTilemap));
        }


        private TileBase[][] CreateTileBasedGrid(InputImageParameters imageParameters)
        {
            TileBase[][] gridOfInputTiles = ArrayExtensions.CreateJaggedArray<TileBase[][]>(imageParameters.Height, imageParameters.Width);

            for(int row=0; row < imageParameters.Height; row++)
            {
                for(int col=0; col < imageParameters.Width; col++)
                {
                    gridOfInputTiles[row][col] = imageParameters.StackOfTiles.Dequeue().Tile;
                }
            }

            return gridOfInputTiles;
        }
    }
}