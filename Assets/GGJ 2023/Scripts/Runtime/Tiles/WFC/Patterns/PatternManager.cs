using UnityEngine;
using System.Collections.Generic;

namespace GGJRuntime
{
    public class PatternManager
    {
        private Dictionary<int, PatternData> patternDataIndexDictionary = new Dictionary<int, PatternData>();
        private Dictionary<int, PatternNeighbors> patternPossibleNeighborDictionary = new Dictionary<int, PatternNeighbors>();
        private int patternSize = -1;
        private IFindNeighborStrategy strategy;


        public PatternManager(int patternSize)
        {
            this.patternSize = patternSize;
        }


        public void ProcessGrid<T>(ValuesManager<T> valuesManager, bool equalWeights, string strategyName=null)
        {
            NeighborStrategyFactory strategyFactory = new NeighborStrategyFactory();

            strategy = strategyFactory.CreateInstance(string.IsNullOrEmpty(strategyName) ? patternSize.ToString() : strategyName);

            CreatePatterns(valuesManager, strategy, equalWeights);
        }


        public int[][] ConvertPatternToValues<T>(int[][] outputValues)
        {
            int patternOutputWidth = outputValues[0].Length;
            int patternOutputHeight = outputValues.Length;
            int valueGridWidth = patternOutputWidth + patternSize - 1;
            int valueGridHeight = patternOutputHeight + patternSize - 1;
            int[][] valueGrid = ArrayExtensions.CreateJaggedArray<int[][]>(valueGridHeight, valueGridWidth);

            for(int row=0; row < patternOutputHeight; row++)
            {
                for(int col=0; col < patternOutputWidth; col++)
                {
                    Pattern pattern = GetPatternDataFromIndex(outputValues[row][col]).Pattern;

                    GetPatternValues(patternOutputWidth, patternOutputHeight, valueGrid, row, col, pattern);
                }
            }

            return valueGrid;
        }


        private void GetPatternValues(int patternOutputWidth, int patternOutputHeight, int[][] valueGrid, int row, int col, Pattern pattern)
        {
            if(row == patternOutputHeight - 1 && col == patternOutputWidth - 1)
            {
                for(int row1=0; row1 < patternSize; row1++)
                {
                    for(int col1=0; col1 < patternSize; col1++)
                    {
                        valueGrid[row + row1][col + col1] = pattern.GetGridValue(col1, row1);
                    }
                }
            }
            else if(row == patternOutputHeight - 1)
            {
                for(int row1=0; row1 < patternSize; row1++)
                {
                    valueGrid[row + row1][col] = pattern.GetGridValue(0, row1);
                }
            }
            else if(col == patternOutputWidth - 1)
            {
                for(int col1=0; col1 < patternSize; col1++)
                {
                    valueGrid[row][col + col1] = pattern.GetGridValue(col1, 0);
                }
            }
            else
            {
                valueGrid[row][col] = pattern.GetGridValue(0, 0);
            }
        }


        private void CreatePatterns<T>(ValuesManager<T> valuesManager, IFindNeighborStrategy strategy, bool equalWeights)
        {
            PatternDataResult patternFinderResult = PatternFinder.GetPatternDataFromGrid(valuesManager, patternSize, equalWeights);
            patternDataIndexDictionary = patternFinderResult.PatternIndexDictionary;

            //
            /*System.Text.StringBuilder output = new System.Text.StringBuilder();
            List<string> outputs = new List<string>();

            for(int row = 0; row < patternFinderResult.GetGridLengthY(); row++)
            {
                output.Clear();

                for(int col = 0; col < patternFinderResult.GetGridLengthX(); col++)
                {
                    output.Append(patternFinderResult.GetIndexAt(col, row) + " ");
                }

                outputs.Add(output.ToString());
            }

            outputs.Reverse();

            outputs.ForEach(s => Debug.Log(s));*/
            //

            GetPatternNeighbors(patternFinderResult, strategy);
        }

        private void GetPatternNeighbors(PatternDataResult patternFinderResult, IFindNeighborStrategy strategy)
        {
            patternPossibleNeighborDictionary = PatternFinder.FindPossibleNeightborsForallPatterns(strategy, patternFinderResult);
        }


        public PatternData GetPatternDataFromIndex(int index)
        {
            return patternDataIndexDictionary[index];
        }


        public HashSet<int> GetPossibleNeighborsForPatternInDirection(int patternIndex, Direction direction)
        {
            return patternPossibleNeighborDictionary[patternIndex].GetNeighborsInDirection(direction);
        }


        public float GetPatternFrequency(int index)
        {
            return GetPatternDataFromIndex(index).FrequencyRelative;
        }


        public float GetPatternFrequencyLog2(int index)
        {
            return GetPatternDataFromIndex(index).FrequencyRelativeLog2;
        }


        public int GetNumberOfPatterns()
        {
            return patternDataIndexDictionary.Count;
        }
    }
}