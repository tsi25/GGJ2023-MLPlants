using UnityEngine;

namespace GGJRuntime
{
    public class WfcCore
    {
        //Larger maps may run into issues with this value. If so, raise value as needed.
        private const int kInnerIterationCap = 2000;

        private OutputGrid outputGrid;
        private PatternManager patternManager;
        private int maxIterations = 0;

        public WfcCore(int outputWidth, int outputHeight, int maxIterations, PatternManager patternManager)
        {
            this.outputGrid = new OutputGrid(outputWidth, outputHeight, patternManager.GetNumberOfPatterns());
            this.patternManager = patternManager;
            this.maxIterations = maxIterations;
        }


        public int[][] CreateOutputGrid()
        {
            int iteration = 0;

            while(iteration < maxIterations)
            {
                CoreSolver solver = new CoreSolver(outputGrid, patternManager);
                int innerIteration = kInnerIterationCap;

                while(!solver.CheckForConflicts() && !solver.CheckIfSolved())
                {
                    Vector2Int position = solver.GetLowestEntropyCell();

                    solver.CollapseCell(position);
                    solver.Propagate();
                    innerIteration--;

                    if(innerIteration <= 0)
                    {
                        Debug.LogWarning("Propagation is taking too long! Consider raising inner iteration cap...");
                        return new int[0][];
                    }
                }

                if(solver.CheckForConflicts())
                {
                    Debug.LogWarning($"WFC: Conflict occured on iteration: {iteration}");
                    iteration++;
                    outputGrid.ResetAllPossibilities();
                    solver = new CoreSolver(outputGrid, patternManager);
                }
                else
                {
                    Debug.Log($"WFC: Solved on: {iteration}");
                    //outputGrid.PrintResultsToConsole();
                    break;
                }
            }

            if(iteration >= maxIterations)
            {
                Debug.LogError($"WFC: Failed to solve tilemap!");
            }

            return outputGrid.GetSolvedOutputGrid();
        }
    }
}