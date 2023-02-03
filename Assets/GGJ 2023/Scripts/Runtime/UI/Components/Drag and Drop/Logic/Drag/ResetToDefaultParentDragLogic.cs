using UnityEngine;

namespace GGJRuntime
{
    [CreateAssetMenu(fileName="Logic - Reset To Default Parent", menuName="GGJ/UI/Drag ang Drop/Logic/Drag/Reset to Default Parent")]
    public class ResetToDefaultParentDragLogic : DragLogic
    {
        public override void DoLogic(Draggable dragTarget, Droppable dropArea)
        {
            dragTarget.ResetToDefaultParent();
        }
    }
}