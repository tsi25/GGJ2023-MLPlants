using UnityEngine;

namespace GGJRuntime
{
    public static class MathExtensions
    {
        public static int Wrap(this int value, int min, int max)
        {
            if(value >= min && value <= max) return value;

            int delta = max - min;

            while(value < min) value += delta;
            while(value > max) value -= delta;

            return value;
        }


        public static int WrapExclusive(this int value, int min, int max)
        {
            if(value >= min && value < max) return value;

            int delta = max - min;

            while(value < min) value += delta;
            while(value >= max) value -= delta;

            return value;
        }


        public static float Wrap(this float value, float min, float max)
        {
            if(value >= min && value <= max) return value;

            float delta = max - min;

            while(value < min) value += delta;
            while(value > max) value -= delta;

            return value;
        }
    }
}