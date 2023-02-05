using UnityEngine;

namespace GGJRuntime
{
    [CreateAssetMenu(fileName="Logic - Set Object Value", menuName="GGJ/UI/Drag ang Drop/Logic/Drag/Set Object Value")]
    public class SetObjectValue : DragLogic
    {
        public event System.Action<SoilType> onUpdate = delegate { };

        public override void DoLogic(Draggable dragTarget, Droppable dropArea)
        {
            if(dragTarget is ConditionDraggable)
            {
                onUpdate.Invoke(((ConditionDraggable)dragTarget).Type);
            }
        }
    }
}