using UnityEngine;

namespace GGJRuntime
{
    [CreateAssetMenu(fileName="Logic - Evaluate Dropped Object", menuName="GGJ/UI/Drag ang Drop/Logic/Drop/Evaluate Dropped Object")]
    public class EvaluateDroppedObjectLogic : DropLogic
    {
        public override bool DoLogic(Draggable dragTarget, Droppable dropArea)
        {
            //TODO:  interface with scriptable objects to set rules.

            return true;
        }
    }
}