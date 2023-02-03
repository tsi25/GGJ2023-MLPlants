using UnityEngine;

namespace GGJRuntime
{
    public abstract class DropLogic : ScriptableObject
    {
        public abstract bool DoLogic(Draggable dragTarget, Droppable dropArea);
    }
}