using UnityEngine;
using UnityEngine.Tilemaps;

namespace GGJRuntime
{
    public class TileBaseValue : IValue<TileBase>
    {
        public TileBase Value { get; private set; }

        public TileBaseValue(TileBase value)
        {
            this.Value = value;
        }


        public bool Equals(IValue<TileBase> a, IValue<TileBase> b)
        {
            return a == b;
        }

        public bool Equals(IValue<TileBase> other)
        {
            return other.Value == this.Value;
        }

        public int GetHashCode(IValue<TileBase> obj)
        {
            return obj.GetHashCode();
        }


        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}