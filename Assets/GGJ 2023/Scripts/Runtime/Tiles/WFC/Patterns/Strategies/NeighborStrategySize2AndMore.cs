using UnityEngine;
using System.Collections.Generic;
using System;

namespace GGJRuntime
{
    public class NeighborStrategySize2AndMore : IFindNeighborStrategy
    {
        public Dictionary<int, PatternNeighbors> FindNeighbors(PatternDataResult patternFinderResult)
        {
            Dictionary<int, PatternNeighbors> result = new Dictionary<int, PatternNeighbors>();

            foreach(var check in patternFinderResult.PatternIndexDictionary)
            {
                foreach(var possibleNeighborForPattern in patternFinderResult.PatternIndexDictionary)
                {
                    FindNeighborsInAllDirections(result, check, possibleNeighborForPattern);
                }
            }

            return result;
        }

        private void FindNeighborsInAllDirections(Dictionary<int, PatternNeighbors> result, KeyValuePair<int, PatternData> patternDataToCheck, KeyValuePair<int, PatternData> possibleNeighborForPattern)
        {
            //TODO: Cleanup later...
            Direction[] directions = (Direction[])Enum.GetValues(typeof(Direction));

            foreach(var direction in directions)
            {
                if(patternDataToCheck.Value.CompareGrid(direction, possibleNeighborForPattern.Value))
                {
                    if(!result.ContainsKey(patternDataToCheck.Key))
                    {
                        result.Add(patternDataToCheck.Key, new PatternNeighbors());
                    }

                    result[patternDataToCheck.Key].AddPatternToDictionary(direction, possibleNeighborForPattern.Key);
                }
            }
        }
    }
}