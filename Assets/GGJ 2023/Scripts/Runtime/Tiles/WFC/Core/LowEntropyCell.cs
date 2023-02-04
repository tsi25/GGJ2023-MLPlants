using UnityEngine;
using System;
using System.Collections.Generic;

namespace GGJRuntime
{
    public class LowEntropyCell : IComparable<LowEntropyCell>, IEqualityComparer<LowEntropyCell>
    {
        public Vector2Int Position { get; set; }
        public float Entropy { get; set; }

        private float SmallEntropyNoise { get; set; }

        public LowEntropyCell(Vector2Int position, float entropy)
        {
            Position = position;
            Entropy = entropy;

            SmallEntropyNoise = UnityEngine.Random.Range(0.001f, 0.005f);
        }

        public int CompareTo(LowEntropyCell other)
        {
            if(Entropy > other.Entropy) return 1;
            else if(Entropy < other.Entropy) return -1;

            return 0;
        }


        public bool Equals(LowEntropyCell a, LowEntropyCell b)
        {
            return a.Position.x == b.Position.x && a.Position.y == b.Position.y;
        }


        public int GetHashCode(LowEntropyCell obj)
        {
            return obj.GetHashCode();
        }


        public override int GetHashCode()
        {
            return Position.GetHashCode();
        }
    }
}