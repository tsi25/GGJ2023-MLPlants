using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace GGJRuntime
{
    public class CoreHelper
    {
        private float totalFrequency = 0f;
        private float totalFrequencyLog = 0f;
        private PatternManager patternManager;

        public CoreHelper(PatternManager manager)
        {
            patternManager = manager;

            /*for(int i=0; i < patternManager.GetNumberOfPatterns(); i++)
            {
                totalFrequency += patternManager.GetPatternFrequency(i);
            }

            totalFrequencyLog = Mathf.Log(totalFrequency, 2);*/
        }


        public int SelectSolutionPatternFromFrequency(List<int> possibleValues)
        {
            List<float> valueFrequenciesFractions = GetListOfWeightsFromIndices(possibleValues);
            float randomValue = Random.Range(0f, valueFrequenciesFractions.Sum());
            float sum = 0;
            int index = 0;

            foreach(var item in valueFrequenciesFractions)
            {
                sum += item;

                if(randomValue <= sum)
                {
                    return index;
                }

                index++;
            }

            return index - 1;
        }


        public List<VectorPair> Create4DirectionNeighbors(Vector2Int cellCoordinates, Vector2Int previousCell)
        {
            List<VectorPair> list = new List<VectorPair>()
            {
                new VectorPair(cellCoordinates, cellCoordinates + new Vector2Int(1, 0), previousCell, Direction.Right),
                new VectorPair(cellCoordinates, cellCoordinates + new Vector2Int(-1, 0), previousCell, Direction.Left),
                new VectorPair(cellCoordinates, cellCoordinates + new Vector2Int(0, 1), previousCell, Direction.Up),
                new VectorPair(cellCoordinates, cellCoordinates + new Vector2Int(0, -1), previousCell, Direction.Down)
            };

            return list;
        }


        public List<VectorPair> Create4DirectionNeighbors(Vector2Int cellCoordinate)
        {
            return Create4DirectionNeighbors(cellCoordinate, cellCoordinate);
        }


        public float CalculateEntropy(Vector2Int position, OutputGrid outputGrid)
        {
            float sum = 0;
            HashSet<int> set = outputGrid.GetPossibleValueForPosition(position);

            foreach(var possibleIndex in set)
            {
                totalFrequency += patternManager.GetPatternFrequency(possibleIndex);
                sum += patternManager.GetPatternFrequencyLog2(possibleIndex);
            }

            totalFrequencyLog = Mathf.Log(totalFrequency, 2);

            return totalFrequencyLog - (sum / totalFrequency);
        }


        public List<VectorPair> CheckIfNeighborsAreCollapsed(VectorPair pairToCheck, OutputGrid outputGrid)
        {
            return Create4DirectionNeighbors(pairToCheck.CellToPropagatePosition, pairToCheck.BaseCellPosition)
                .Where(x => outputGrid.CheckIfValidPosition(x.CellToPropagatePosition) && !outputGrid.CheckIfCellIsCollapsed(x.CellToPropagatePosition))
                .ToList();
        }


        public bool CheckCellSolutionForCollision(Vector2Int cellCoordinates, OutputGrid outputGrid)
        {
            List<VectorPair> neighbors = Create4DirectionNeighbors(cellCoordinates);

            foreach(VectorPair neighbor in neighbors)
            {
                if(!outputGrid.CheckIfValidPosition(neighbor.CellToPropagatePosition))
                {
                    continue;
                }

                HashSet<int> possibleIndices = new HashSet<int>();
                HashSet<int> possibleValues = outputGrid.GetPossibleValueForPosition(neighbor.CellToPropagatePosition);

                foreach(int patternIndexAtNeighbor in possibleValues)
                {
                    HashSet<int> possibleNeighborsForBase = patternManager.GetPossibleNeighborsForPatternInDirection(patternIndexAtNeighbor, neighbor.DirectionFromBase.GetOppositeDirection());

                    possibleIndices.UnionWith(possibleNeighborsForBase);
                }

                if(!possibleIndices.Contains(outputGrid.GetPossibleValueForPosition(cellCoordinates).First()))
                {
                    return true;
                }
            }

            return false;
        }


        private List<float> GetListOfWeightsFromIndices(List<int> possibleValues)
        {
            List<float> valueFrequencies = possibleValues.Aggregate(new List<float>(), (acc, val) =>
            {
                acc.Add(patternManager.GetPatternFrequency(val));
                return acc;
            },
            acc => acc).ToList();

            return valueFrequencies;
        }
    }
}