using UnityEngine;
using System.Collections.Generic;

namespace GGJRuntime
{
    public class NeighborStrategySize1Default : IFindNeighborStrategy
    {
        public Dictionary<int, PatternNeighbors> FindNeighbors(PatternDataResult patternFinderResult)
        {
            Dictionary<int, PatternNeighbors> result = new Dictionary<int, PatternNeighbors>();

            FindNeighborsForEachPattern(patternFinderResult, result);

            return result;
        }

        private void FindNeighborsForEachPattern(PatternDataResult patternFinderResult, Dictionary<int, PatternNeighbors> result)
        {
            for(int row=0; row < patternFinderResult.GetGridLengthY(); row++)
            {
                for(int col=0; col < patternFinderResult.GetGridLengthX(); col++)
                {
                    PatternNeighbors neighbors = PatternFinder.CheckNeighborsInEachDirection(col, row, patternFinderResult);

                    PatternFinder.AddNeighborsToDictionary(result, patternFinderResult.GetIndexAt(col, row), neighbors);
                }
            }
        }
    }
}