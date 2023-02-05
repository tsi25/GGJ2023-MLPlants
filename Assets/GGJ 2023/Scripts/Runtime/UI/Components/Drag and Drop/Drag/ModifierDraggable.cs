using UnityEngine;

namespace GGJRuntime
{
    public class ModifierDraggable : Draggable
    {
        [field: SerializeField]
        public ModifierType Type { get; private set; } = ModifierType.None;
    }
}