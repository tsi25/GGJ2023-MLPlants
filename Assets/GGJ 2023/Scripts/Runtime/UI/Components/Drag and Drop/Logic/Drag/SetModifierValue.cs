using UnityEngine;

namespace GGJRuntime
{
    [CreateAssetMenu(fileName="Logic - Set Modifier Value", menuName="GGJ/UI/Drag ang Drop/Logic/Drag/Set Modifier Value")]
    public class SetModifierValue : DragLogic
    {
        public event System.Action<ModifierType> onUpdate = delegate { };

        public override void DoLogic(Draggable dragTarget, Droppable dropArea)
        {
            if(dragTarget is ConditionDraggable)
            {
                onUpdate.Invoke(((ModifierDraggable)dragTarget).Type);
            }
        }
    }
}
