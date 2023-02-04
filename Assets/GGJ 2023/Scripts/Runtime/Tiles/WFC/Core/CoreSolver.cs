using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace GGJRuntime
{
    public class CoreSolver
    {
        private PatternManager patternManager;
        private OutputGrid outputGrid;
        private CoreHelper coreHelper;
        private PropagationHelper propagationHelper;


        public CoreSolver(OutputGrid outputGrid, PatternManager patternManager)
        {
            this.outputGrid = outputGrid;
            this.patternManager = patternManager;

            this.coreHelper = new CoreHelper(patternManager);
            this.propagationHelper = new PropagationHelper(outputGrid, coreHelper);
        }


        public Vector2Int GetLowestEntropyCell()
        {
            if(propagationHelper.LowEntropySet.Count <= 0)
            {
                return outputGrid.GetRandomCell();
            }

            LowEntropyCell lowestEntropyElement = propagationHelper.LowEntropySet.First();
            Vector2Int returnVector = lowestEntropyElement.Position;
            propagationHelper.LowEntropySet.Remove(lowestEntropyElement);

            return returnVector;
        }


        public bool CheckIfSolved()
        {
            return outputGrid.CheckIfGridIsSolved();
        }


        public bool CheckForConflicts()
        {
            return propagationHelper.CheckForConflicts();
        }


        public void Propagate()
        {
            while(propagationHelper.PairsToPropagate.Count > 0)
            {
                VectorPair propagatePair = propagationHelper.PairsToPropagate.Dequeue();

                if(propagationHelper.CheckIfPairShouldBeProcessed(propagatePair))
                {
                    ProcessCell(propagatePair);
                }

                if(propagationHelper.CheckForConflicts() || outputGrid.CheckIfGridIsSolved())
                {
                    return;
                }
            }

            if(propagationHelper.CheckForConflicts() && propagationHelper.PairsToPropagate.Count == 0 && propagationHelper.LowEntropySet.Count == 0)
            {
                propagationHelper.SetConflictFlag();
            }
        }


        public void CollapseCell(Vector2Int cellCoordinates)
        {
            var possibleValue = outputGrid.GetPossibleValueForPosition(cellCoordinates).ToList();

            if(possibleValue.Count == 0 || possibleValue.Count == 1)
            {
                return;
            }

            int index = coreHelper.SelectSolutionPatternFromFrequency(possibleValue);

            outputGrid.SetPatternOnPosition(cellCoordinates.x, cellCoordinates.y, possibleValue[index]);

            if(!coreHelper.CheckCellSolutionForCollision(cellCoordinates, outputGrid))
            {
                propagationHelper.AddNewPairsToPropagateQueue(cellCoordinates, cellCoordinates);
            }
            else
            {
                propagationHelper.SetConflictFlag();
            }
        }


        private void ProcessCell(VectorPair propagatePair)
        {
            if(outputGrid.CheckIfCellIsCollapsed(propagatePair.CellToPropagatePosition))
            {
                propagationHelper.EnqueueUncollapseNeighbors(propagatePair);
            }
            else
            {
                PropagateNeighbor(propagatePair);
            }
        }

        private void PropagateNeighbor(VectorPair propagatePair)
        {
            HashSet<int> possibleValuesAtNeighbor = outputGrid.GetPossibleValueForPosition(propagatePair.CellToPropagatePosition);
            int startCount = possibleValuesAtNeighbor.Count;

            RemoveImpossibleNeighbors(propagatePair, possibleValuesAtNeighbor);

            int newPossiblePatternCount = possibleValuesAtNeighbor.Count;
            propagationHelper.AnalyzePropagationResult(propagatePair, startCount, newPossiblePatternCount);
        }

        private void RemoveImpossibleNeighbors(VectorPair propagatePair, HashSet<int> possibleValuesAtNeighbor)
        {
            HashSet<int> possibleIndices = new HashSet<int>();
            HashSet<int> set = outputGrid.GetPossibleValueForPosition(propagatePair.BaseCellPosition);

            foreach(int patternIndexAtBase in set)
            {
                HashSet<int> possibleNeighborsForBase = patternManager.GetPossibleNeighborsForPatternInDirection(patternIndexAtBase, propagatePair.DirectionFromBase);

                possibleIndices.UnionWith(possibleNeighborsForBase);
            }

            possibleValuesAtNeighbor.IntersectWith(possibleIndices);
        }
    }
}