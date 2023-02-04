using UnityEngine;

namespace GGJRuntime
{
    public interface IOutputCreator<T>
    {
        T OutputImage { get; }

        void CreateOutput(PatternManager manager, int[][] outputValues, int width, int height);
    }
}