using UnityEngine;
using System.Collections.Generic;

namespace GGJRuntime
{
    public class PatternDataResult
    {
        private int[][] patternIndicesGrid;

        public Dictionary<int, PatternData> PatternIndexDictionary { get; internal set; }

        public PatternDataResult(int[][] patternIndicesGrid, Dictionary<int, PatternData> patternIndexDictionary)
        {
            this.patternIndicesGrid = patternIndicesGrid;
            PatternIndexDictionary = patternIndexDictionary;
        }


        public int GetGridLengthX()
        {
            return patternIndicesGrid[0].Length;
        }


        public int GetGridLengthY()
        {
            return patternIndicesGrid.Length;
        }


        public int GetIndexAt(int x, int y)
        {
            return patternIndicesGrid[y][x];
        }

        public int GetNeighborInDirection(int x, int y, Direction direction)
        {
            if(!patternIndicesGrid.CheckJaggedArray2IfIndexIsValid(x, y))
            {
                return -1;
            }

            switch(direction)
            {
                case Direction.Up:
                    if(patternIndicesGrid.CheckJaggedArray2IfIndexIsValid(x, y + 1))
                    {
                        return GetIndexAt(x, y + 1);
                    }
                    return -1;
                case Direction.Down:
                    if(patternIndicesGrid.CheckJaggedArray2IfIndexIsValid(x, y - 1))
                    {
                        return GetIndexAt(x, y - 1);
                    }
                    return -1;
                case Direction.Left:
                    if(patternIndicesGrid.CheckJaggedArray2IfIndexIsValid(x - 1, y))
                    {
                        return GetIndexAt(x - 1, y);
                    }
                    return -1;
                case Direction.Right:
                    if(patternIndicesGrid.CheckJaggedArray2IfIndexIsValid(x + 1, y))
                    {
                        return GetIndexAt(x + 1, y);
                    }
                    return -1;
            }

            return -1;
        }
    }
}