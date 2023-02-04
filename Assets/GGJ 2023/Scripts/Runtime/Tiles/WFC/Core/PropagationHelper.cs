using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace GGJRuntime
{
    public class PropagationHelper
    {
        private OutputGrid outputGrid;
        private CoreHelper coreHelper;
        private bool cellWithNoSolutionPresent;
        private SortedSet<LowEntropyCell> lowEntropySet = new SortedSet<LowEntropyCell>();
        private Queue<VectorPair> pairsToPropagate = new Queue<VectorPair>();

        public SortedSet<LowEntropyCell> LowEntropySet { get => lowEntropySet; }
        public Queue<VectorPair> PairsToPropagate { get => pairsToPropagate; }

        public PropagationHelper(OutputGrid outputGrid, CoreHelper coreHelper)
        {
            this.outputGrid = outputGrid;
            this.coreHelper = coreHelper;
        }


        public bool CheckIfPairShouldBeProcessed(VectorPair propagationPair)
        {
            return outputGrid.CheckIfValidPosition(propagationPair.CellToPropagatePosition) && !propagationPair.AreWeCheckingPreviousCellAgain();
        }


        public void AnalyzePropagationResult(VectorPair propagatePair, int startCount, int newPossiblePatternCount)
        {
            if(newPossiblePatternCount > 1 && startCount > newPossiblePatternCount)
            {
                AddNewPairsToPropagateQueue(propagatePair.CellToPropagatePosition, propagatePair.BaseCellPosition);
                AddToLowEntropySet(propagatePair.CellToPropagatePosition);
            }

            if(newPossiblePatternCount == 0)
            {
                cellWithNoSolutionPresent = true;
            }

            if(newPossiblePatternCount == 1)
            {
                cellWithNoSolutionPresent = coreHelper.CheckCellSolutionForCollision(propagatePair.CellToPropagatePosition, outputGrid);
            }
        }

        public void EnqueueUncollapseNeighbors(VectorPair propagatePair)
        {
            List<VectorPair> uncollapsedNeighbors = coreHelper.CheckIfNeighborsAreCollapsed(propagatePair, outputGrid);

            foreach(var item in uncollapsedNeighbors)
            {
                pairsToPropagate.Enqueue(item);
            }
        }


        public bool CheckForConflicts()
        {
            return cellWithNoSolutionPresent;
        }


        public void SetConflictFlag()
        {
            cellWithNoSolutionPresent = true;
        }


        public void AddToLowEntropySet(Vector2Int cellToPropagatePosition)
        {
            LowEntropyCell elementOfLowEntropySet = lowEntropySet.Where(x => x.Position == cellToPropagatePosition).FirstOrDefault();

            if(elementOfLowEntropySet == null && !outputGrid.CheckIfCellIsCollapsed(cellToPropagatePosition))
            {
                float entropy = coreHelper.CalculateEntropy(cellToPropagatePosition, outputGrid);

                lowEntropySet.Add(new LowEntropyCell(cellToPropagatePosition, entropy));
            }
            else
            {
                lowEntropySet.Remove(elementOfLowEntropySet);
                elementOfLowEntropySet.Entropy = coreHelper.CalculateEntropy(cellToPropagatePosition, outputGrid);
                lowEntropySet.Add(elementOfLowEntropySet);
            }
        }


        public void AddNewPairsToPropagateQueue(Vector2Int cellToPropagatePosition, Vector2Int baseCellPosition)
        {
            List<VectorPair> list = coreHelper.Create4DirectionNeighbors(cellToPropagatePosition, baseCellPosition);

            foreach(var item in list)
            {
                pairsToPropagate.Enqueue(item);
            }
        }
    }
}