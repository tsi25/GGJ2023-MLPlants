using UnityEngine;
using System.Collections.Generic;
using System;

namespace GGJRuntime
{
    public static class PatternFinder
    {
        public static PatternDataResult GetPatternDataFromGrid<T>(ValuesManager<T> valuesManager, int patternSize, bool equalWeights)
        {
            Dictionary<string, PatternData> patternHashCodeDictionary = new Dictionary<string, PatternData>();
            Dictionary<int, PatternData> patternIndexDictionary = new Dictionary<int, PatternData>();
            Vector2Int sizeOfGrid = valuesManager.GetGridSize();
            Vector2Int patternGridSize = Vector2Int.zero;
            int rowMin = -1;
            int rowMax = -1;
            int colMin = -1;
            int colMax = -1;

            if(patternSize < 3)
            {
                patternGridSize.x = sizeOfGrid.x + 3 - patternSize;
                patternGridSize.y = sizeOfGrid.y + 3 - patternSize;
                rowMax = patternGridSize.y - 1;
                colMax = patternGridSize.x - 1;
            }
            else
            {
                patternGridSize.x = sizeOfGrid.x + patternSize - 1;
                patternGridSize.y = sizeOfGrid.y + patternSize - 1;
                rowMin = 1 - patternSize;
                colMin = 1 - patternSize;
                rowMax = sizeOfGrid.y;
                colMax = sizeOfGrid.x;
            }

            int[][] patternIndecesGrid = ArrayExtensions.CreateJaggedArray<int[][]>(patternGridSize.y, patternGridSize.x);
            int totalFrequency = 0;
            int patternIndex = 0;

            for(int row=rowMin; row < rowMax; row++)
            {
                for(int col=colMin; col < colMax; col++)
                {
                    int[][] gridValues = valuesManager.GetPatternValuesFromGridAt(col, row, patternSize);
                    string hashValue = HashCodeUtility.CalculateHashCode(gridValues);

                    if(!patternHashCodeDictionary.ContainsKey(hashValue))
                    {
                        Pattern pattern = new Pattern(gridValues, hashValue, patternIndex);
                        patternIndex++;
                        AddNewPattern(patternHashCodeDictionary, patternIndexDictionary, hashValue, pattern);
                    }
                    else
                    {
                        if(!equalWeights)
                        {
                            patternIndexDictionary[patternHashCodeDictionary[hashValue].Pattern.Index].AddToFrequency();
                        }
                    }

                    totalFrequency++;

                    if(patternSize < 3)
                    {
                        patternIndecesGrid[row + 1][col + 1] = patternHashCodeDictionary[hashValue].Pattern.Index;
                    }
                    else
                    {
                        patternIndecesGrid[row + patternSize -1][col + patternSize - 1] = patternHashCodeDictionary[hashValue].Pattern.Index;
                    }
                }
            }

            CalculateRelativeFrequency(patternIndexDictionary, totalFrequency);

            return new PatternDataResult(patternIndecesGrid, patternIndexDictionary);
        }


        public static PatternNeighbors CheckNeighborsInEachDirection(int x, int y, PatternDataResult patternDataResults)
        {
            PatternNeighbors patternNeighbors = new PatternNeighbors();

            //TODO: Cleanup later...
            Direction[] directions = (Direction[])Enum.GetValues(typeof(Direction));

            foreach(var direction in directions)
            {
                int possiblePatternIndex = patternDataResults.GetNeighborInDirection(x, y, direction);

                if(possiblePatternIndex >= 0)
                {
                    patternNeighbors.AddPatternToDictionary(direction, possiblePatternIndex);
                }
            }

            return patternNeighbors;
        }


        public static void AddNeighborsToDictionary(Dictionary<int, PatternNeighbors> dictionary, int patternIndex, PatternNeighbors neighbors)
        {
            if(!dictionary.ContainsKey(patternIndex))
            {
                dictionary.Add(patternIndex, neighbors);
            }

            dictionary[patternIndex].AddNeighbor(neighbors);
        }


        public static Dictionary<int, PatternNeighbors> FindPossibleNeightborsForallPatterns(IFindNeighborStrategy strategy, PatternDataResult patternFinderResult)
        {
            return strategy.FindNeighbors(patternFinderResult);
        }


        private static void CalculateRelativeFrequency(Dictionary<int, PatternData> patternIndexDictionary, int totalFrequency)
        {
            foreach(var item in patternIndexDictionary.Values)
            {
                item.CalculateRelativeFrequency(totalFrequency);
            }
        }


        private static void AddNewPattern(Dictionary<string, PatternData> patternHashCodeDictionary, Dictionary<int, PatternData> patternIndexDictionary, string hashValue, Pattern pattern)
        {
            PatternData data = new PatternData(pattern);

            patternHashCodeDictionary.Add(hashValue, data);
            patternIndexDictionary.Add(pattern.Index, data);
        }
    }
}