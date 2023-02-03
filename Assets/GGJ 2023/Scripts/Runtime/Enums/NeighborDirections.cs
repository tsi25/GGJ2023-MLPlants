using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJRuntime
{
    // Set up for use with bitmasks in case that's helpful later
    public enum NeighborDirections : byte
    {
        North = 1,                  // 0001   
        East = 2,                   // 0010
        South = 4,                  // 0100
        West = 8,                   // 1000
        NorthEast = 3,              // 0011
        SouthEast = 6,              // 0101
        SouthWest = 12,             // 1100
        NorthWest = 9               // 1001
    }
}
