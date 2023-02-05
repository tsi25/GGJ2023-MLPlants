using UnityEngine;

namespace GGJRuntime
{
    [CreateAssetMenu(fileName="Logic - Set Modifier Value", menuName="GGJ/UI/Drag ang Drop/Logic/Drop/Set Modifier Value")]
    public class SetModifierValueLogic : DropLogic
    {
        public event System.Action<ModifierType> onUpdate = delegate { };

        public override bool DoLogic(Draggable dragTarget, Droppable dropArea)
        {
            if(dragTarget is ConditionDraggable)
            {
                onUpdate.Invoke(((ModifierDraggable)dragTarget).Type);
            }

            return true;
        }
    }
}