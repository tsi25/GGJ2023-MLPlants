using UnityEngine;

namespace GGJRuntime
{
    public class ConditionDraggable : Draggable
    {
        [field: SerializeField]
        public SoilType Type { get; private set; } = SoilType.None;
    }
}