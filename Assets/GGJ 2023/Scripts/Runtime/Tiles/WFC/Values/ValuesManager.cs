using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace GGJRuntime
{
    public class ValuesManager<T>
    {
        private int[][] grid;
        private Dictionary<int, IValue<T>> valueIndexDictionary = new Dictionary<int, IValue<T>>();
        int index = 0;
        
        public ValuesManager(IValue<T>[][] gridOfValues)
        {
            CreateGridOfIndices(gridOfValues);
        }


        public int GetGridValue(int x, int y)
        {
            if(x >= grid[0].Length || x < 0 || y >= grid.Length || y < 0)
            {
                throw new System.IndexOutOfRangeException($"Grid doesn't contain coordinate ({x}, {y})!");
            }

            return grid[y][x];
        }


        public Vector2Int GetGridSize()
        {
            if(grid == null) return Vector2Int.zero;

            return new Vector2Int(grid[0].Length, grid.Length);
        }


        public IValue<T> GetValueFromIndex(int index)
        {
            if(valueIndexDictionary.TryGetValue(index, out IValue<T> value))
            {
                return value;
            }

            throw new System.Exception($"No index of [{index}] found!");
        }


        public int GetGridValuesIncludingOffset(int x, int y)
        {
            x = x.WrapExclusive(0, grid[0].Length);
            y = y.WrapExclusive(0, grid.Length);

            return GetGridValue(x, y);

            /*if(x < 0 && y < 0) //https://www.youtube.com/watch?v=w62y9swx4zg
            {
                return GetGridValue(xMax + x, yMax + y);
            }*/
        }


        public int[][] GetPatternValuesFromGridAt(int x, int y, int patternSize)
        {
            int[][] arrayToReturn = ArrayExtensions.CreateJaggedArray<int[][]>(patternSize, patternSize);

            for(int row=0; row < patternSize; row++)
            {
                for(int col=0; col < patternSize; col++)
                {
                    arrayToReturn[row][col] = GetGridValuesIncludingOffset(x + col, y + row);
                }
            }

            return arrayToReturn;
        }


        private void CreateGridOfIndices(IValue<T>[][] gridOfValues)
        {
            grid = ArrayExtensions.CreateJaggedArray<int[][]>(gridOfValues.Length, gridOfValues[0].Length);

            for(int row=0; row < gridOfValues.Length; row++)
            {
                for(int col=0; col < gridOfValues[0].Length; col++)
                {
                    SetIndexToGridPosition(gridOfValues, row, col);
                }
            }
        }


        private void SetIndexToGridPosition(IValue<T>[][] gridOfValues, int row, int col)
        {
            if(valueIndexDictionary.ContainsValue(gridOfValues[row][col]))
            {
                var key = valueIndexDictionary.FirstOrDefault(x => x.Value.Equals(gridOfValues[row][col]));
                grid[row][col] = key.Key;
            }
            else
            {
                grid[row][col] = index;

                valueIndexDictionary.Add(grid[row][col], gridOfValues[row][col]);

                index++;
            }
        }
    }
}