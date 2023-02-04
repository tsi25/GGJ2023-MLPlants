using UnityEngine;
using System.Collections.Generic;

namespace GGJRuntime
{
    public interface IFindNeighborStrategy
    {
        Dictionary<int, PatternNeighbors> FindNeighbors(PatternDataResult patternFinderResult);
    }
}