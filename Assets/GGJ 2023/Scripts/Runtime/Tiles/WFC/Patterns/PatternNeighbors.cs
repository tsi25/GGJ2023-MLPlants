using UnityEngine;
using System.Collections.Generic;

namespace GGJRuntime
{
    public class PatternNeighbors
    {
        public Dictionary<Direction, HashSet<int>> directionPatternNeighborDictionary = new Dictionary<Direction, HashSet<int>>();


        public HashSet<int> GetNeighborsInDirection(Direction direction)
        {
            if(directionPatternNeighborDictionary.TryGetValue(direction, out HashSet<int> value))
            {
                return value;
            }

            return new HashSet<int>();
        }


        public void AddPatternToDictionary(Direction direction, int patternIndex)
        {
            if(directionPatternNeighborDictionary.ContainsKey(direction))
            {
                directionPatternNeighborDictionary[direction].Add(patternIndex);
            }
            else
            {
                directionPatternNeighborDictionary.Add(direction, new HashSet<int>() { patternIndex });
            }
        }


        public void AddNeighbor(PatternNeighbors neighbors)
        {
            foreach(var item in neighbors.directionPatternNeighborDictionary)
            {
                if(!directionPatternNeighborDictionary.ContainsKey(item.Key))
                {
                    directionPatternNeighborDictionary.Add(item.Key, new HashSet<int>());
                }

                directionPatternNeighborDictionary[item.Key].UnionWith(item.Value);
            }
        }
    }
}