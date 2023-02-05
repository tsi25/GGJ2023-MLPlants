using UnityEngine;

namespace GGJRuntime
{
    [CreateAssetMenu(fileName="Logic - Set Object Value", menuName="GGJ/UI/Drag ang Drop/Logic/Drop/Set Object Value")]
    public class SetObjectValueLogic : DropLogic
    {
        public event System.Action<SoilType> onUpdate = delegate { };

        public override bool DoLogic(Draggable dragTarget, Droppable dropArea)
        {
            if(dragTarget is ConditionDraggable)
            {
                onUpdate.Invoke(((ConditionDraggable)dragTarget).Type);
            }

            return true;
        }
    }
}