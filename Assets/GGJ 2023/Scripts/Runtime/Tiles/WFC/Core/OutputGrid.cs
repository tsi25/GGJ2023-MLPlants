using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace GGJRuntime
{
    public class OutputGrid
    {
        private int maxNumberOfPatterns = 0;
        private Dictionary<int, HashSet<int>> indexPossiblePatternDictionary = new Dictionary<int, HashSet<int>>();

        public int Width { get; }
        public int Height { get; }

        public OutputGrid(int width, int height, int numberOfPatterns)
        {
            this.Width = width;
            this.Height = height;
            this.maxNumberOfPatterns = numberOfPatterns;

            ResetAllPossibilities();
        }


        public bool CheckCellExists(Vector2Int position)
        {
            int index = GetIndexFromCoorinates(position);

            return indexPossiblePatternDictionary.ContainsKey(index);
        }


        public bool CheckIfCellIsCollapsed(Vector2Int position)
        {
            return GetPossibleValueForPosition(position).Count <= 1;
        }


        public bool CheckIfGridIsSolved()
        {
            return !indexPossiblePatternDictionary.Any(x => x.Value.Count > 1);
        }


        public bool CheckIfValidPosition(Vector2Int position)
        {
            return ArrayExtensions.ValidateCoordinates(position.x, position.y, Width, Height);
        }


        public Vector2Int GetRandomCell()
        {
            int randomIndex = Random.Range(0, indexPossiblePatternDictionary.Count);

            return GetCoordsFromIndex(randomIndex);
        }


        public int[][] GetSolvedOutputGrid()
        {
            int[][] returnGrid = ArrayExtensions.CreateJaggedArray<int[][]>(Height, Width);

            if(!CheckIfGridIsSolved())
            {
                return ArrayExtensions.CreateJaggedArray<int[][]>(0, 0);
            }

            for(int row=0; row < Height; row++)
            {
                for(int col=0; col < Width; col++)
                {
                    int index = GetIndexFromCoorinates(new Vector2Int(col, row));

                    returnGrid[row][col] = indexPossiblePatternDictionary[index].First();
                }
            }

            return returnGrid;
        }

        public void PrintResultsToConsole()
        {
            System.Text.StringBuilder output = new System.Text.StringBuilder();
            List<string> outputs = new List<string>();

            for(int row = 0; row < Height; row++)
            {
                output.Clear();

                for(int col = 0; col < Width; col++)
                {
                    var result = GetPossibleValueForPosition(new Vector2Int(col, row));

                    if(result.Count == 1)
                    {
                        output.Append(result.First() + " ");
                    }
                    else
                    {
                        System.Text.StringBuilder resultOutput = new System.Text.StringBuilder();

                        foreach(int item in result)
                        {
                            resultOutput.Append($"{item},");
                        }

                        output.Append(resultOutput.ToString());
                    }
                }

                outputs.Add(output.ToString());
            }

            outputs.Reverse();

            outputs.ForEach(s => Debug.Log(s));

            Debug.Log("------------");
        }


        public HashSet<int> GetPossibleValueForPosition(Vector2Int position)
        {
            int index = GetIndexFromCoorinates(position);

            if(indexPossiblePatternDictionary.ContainsKey(index))
            {
                return indexPossiblePatternDictionary[index];
            }

            return new HashSet<int>();
        }


        public void SetPatternOnPosition(int x, int y, int patternIndex)
        {
            int index = GetIndexFromCoorinates(new Vector2Int(x, y));

            indexPossiblePatternDictionary[index] = new HashSet<int>() { patternIndex };
        }


        public void ResetAllPossibilities()
        {
            HashSet<int> allPossiblePatternList = new HashSet<int>();

            allPossiblePatternList.UnionWith(Enumerable.Range(0, this.maxNumberOfPatterns).ToList());

            indexPossiblePatternDictionary.Clear();

            for(int i = 0; i < Height * Width; i++)
            {
                indexPossiblePatternDictionary.Add(i, new HashSet<int>(allPossiblePatternList));
            }
        }


        private Vector2Int GetCoordsFromIndex(int randomIndex)
        {
            return new Vector2Int(randomIndex / Width, randomIndex % Height);
        }


        private int GetIndexFromCoorinates(Vector2Int position)
        {
            return position.x + Width * position.y;
        }
    }
}