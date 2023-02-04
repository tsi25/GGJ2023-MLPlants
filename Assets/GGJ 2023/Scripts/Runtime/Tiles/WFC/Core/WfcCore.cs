using UnityEngine;

namespace GGJRuntime
{
    public class WfcCore
    {
        //Larger maps may run into issues with this value. If so, raise value as needed.
        private const int kInnerIterationCap = 5000;

        public static bool DebugIterations = false;

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
                    if(DebugIterations) Debug.LogWarning($"WFC: Conflict occured on iteration: {iteration}");
                    iteration++;
                    outputGrid.ResetAllPossibilities();
                    solver = new CoreSolver(outputGrid, patternManager);
                }
                else
                {
                    if(DebugIterations) Debug.Log($"WFC: Solved on: {iteration}");
                    //outputGrid.PrintResultsToConsole();
                    break;
                }
            }

            if(iteration >= maxIterations)
            {
                if(DebugIterations) Debug.LogError($"WFC: Failed to solve tilemap!");
            }

            return outputGrid.GetSolvedOutputGrid();
        }
    }
}