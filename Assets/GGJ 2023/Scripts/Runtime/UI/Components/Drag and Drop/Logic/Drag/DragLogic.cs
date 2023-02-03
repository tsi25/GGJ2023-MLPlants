using UnityEngine;

namespace GGJRuntime
{
    public abstract class DragLogic : ScriptableObject
    {
        public abstract void DoLogic(Draggable draggable);
    }
}